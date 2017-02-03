﻿/*
Copyright (c) 2011, Ryan Hitchman
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
THE POSSIBILITY OF SUCH DAMAGE.
*/

/*

Region File Format

Concept: The minimum unit of storage on hard drives is 4KB. 90% of Minecraft
 chunks are smaller than 4KB. 99% are smaller than 8KB. Write a simple
 container to store chunks in single files in runs of 4KB sectors.

Each region file represents a 32x32 group of chunks. The conversion from
chunk number to region number is floor(coord / 32); a chunk at (30, -3)
would be in region (0, -1), and one at (70, -30) would be at (3, -1).
Region files are named "r.x.z.mcr", where x and z are the region coordinates.

A region file begins with an 8KB header that describes where chunks are stored
in the file and when they were last modified. A 4-byte big-endian integer 
represents sector offsets and sector counts. The chunk offset for a chunk 
located at (x, z) begins at byte 4*(x+z*32) in the file. The bottom byte of 
the chunk offset indicates the number of sectors the chunk takes up,and 
the top 3 bytes represent the sector number of the chunk. Given a chunk
offset o, the chunk data begins at byte 4096*(o/256) and takes up at 
most 4096*(o%256) bytes. A chunk cannot exceed 1MB in size. A chunk offset 
of 0 indicates a missing chunk.

The 4-byte big-endian modification time for a chunk (x,z) begins at byte 
4096+4*(x+z*32) in the file. The time is stored as the number of seconds
since Jan 1, 1970 that the chunk was last written (aka Unix Time).

Chunk data begins with a 4-byte big-endian integer representing the chunk data
length in bytes, not counting the length field. The length must be smaller than
4096 times the number of sectors. The next byte is a version number, to allow
backwards-compatible updates to how chunks are encoded.

A version number of 1 is never used, for obscure historical reasons.

A version number of 2 represents a deflated (zlib compressed) NBT file. The 
deflated data is the chunk length - 1.

*/

#include "stdafx.h"

const int CHUNK_DEFLATE_MAX = 1024 * 64;  // 64KB limit for compressed chunks
const int CHUNK_INFLATE_MAX = 1024 * 128; // 128KB limit for inflated chunks

#define ERROR(x) if(x) { fclose(regionFile); return 0; }

// directory: the base world directory, e.g. "/home/ryan/.minecraft/saves/World1/"
// cx, cz: the chunk's x and z offset
// block: a 32KB buffer to write block data into
// blockLight: a 16KB buffer to write block light into (not skylight)
//
// returns 1 on success, 0 on error
int regionGetBlocks(char* directory, int cx, int cz, unsigned char* block, unsigned char* blockLight)
{
    char filename[256];
    FILE* regionFile;

    unsigned char buf[5];
    int sectorNumber, offset, chunkLength;

    z_stream strm;
    int status;
    unsigned char in[CHUNK_DEFLATE_MAX], out[CHUNK_INFLATE_MAX];

    // open the region file
    snprintf(filename, 256, "%s/region/r.%d.%d.mcr", directory, cx>>5, cz>>5);

regionFile = fopen(filename, "r");
    if (regionFile == NULL)
        return 0;

    // seek to the chunk offset
    ERROR(fseek(regionFile, 4*((cx&31)+(cz&31)*32), SEEK_SET));

    // get the chunk offset
    ERROR(fread(buf, 4, 1, regionFile) != 1);

    sectorNumber = buf[3]; // how many 4096B sectors the chunk takes up
    offset = buf[0]<<16|buf[1]<<8|buf[2]; // 4KB sector the chunk is in

    ERROR(offset == 0); // an empty chunk

    ERROR(fseek(regionFile, 4096*offset, SEEK_SET));

    ERROR(fread(buf, 5, 1, regionFile) != 1); // get chunk length & version
    
    chunkLength = buf[0]<<24|buf[1]<<16|buf[2]<<8|buf[3];

    // sanity check chunk size
    ERROR(chunkLength > sectorNumber* 4096 || chunkLength > CHUNK_DEFLATE_MAX);

    // only handle zlib-compressed chunks (v2)
    ERROR(buf[4] != 2);

    // read compressed chunk data
    ERROR(fread(in, chunkLength - 1, 1, regionFile) != 1);

    fclose(regionFile);

// decompress chunk
strm.zalloc = (alloc_func)NULL;
    strm.zfree = (free_func)NULL;
    strm.opaque = NULL;

    strm.next_out = out;
    strm.avail_out = CHUNK_INFLATE_MAX;
    strm.avail_in = chunkLength - 1;
    strm.next_in = in;

    inflateInit(&strm);
status = inflate(&strm, Z_FINISH); // decompress in one step
    inflateEnd(&strm);

    if (status != Z_STREAM_END) // error inflating (not enough space?)
        return 0;

    // the uncompressed chunk data is now in "out", with length strm.avail_out

    bfFile bf;
bf.type = BF_BUFFER;
    bf.buf = out;
    bf._offset = 0;
    bf.offset = &bf._offset;

    return nbtGetBlocks(bf, block, blockLight);
}