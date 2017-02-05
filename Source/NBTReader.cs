using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using static MinecraftMapReader.Source.NBTReader;

namespace MinecraftMapReader.Source
{
    public class NBTReader
    {
        public enum TAG
        {
            None = -1,
            End = 0,
            Byte = 1,
            Short = 2,
            Int = 3,
            Long = 4,
            Float = 5,
            Double = 6,
            Byte_Array = 7,
            String = 8,
            List = 9,
            Compound = 10,
            Int_Array = 11
        }
        public static NBTTree readNBT(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                BinaryReader br = new BinaryReader(ms);
                return new NBTTree(br);
            }
        }
    }
    public class NBTTree
    {
        public NBTCompoundTag tree;
        public NBTTree(BinaryReader br)
        {
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            var tag = (TAG)br.ReadByte();
            if (tag != TAG.Compound)
                throw new Exception();
            tree = (NBTCompoundTag)NBTTag.ReadTag(br, true, tag);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class TagTypeAttribute : Attribute
    {
        public TAG[] Types { get; set; }

        public TagTypeAttribute(params TAG[] types)
        {
            Types = types;
        }
    }

    public abstract class NBTTag
    {
        private delegate NBTTag ReadMethod(BinaryReader reader);

        private readonly static Dictionary<TAG, ReadMethod> _sReadMethods
            = new Dictionary<TAG, ReadMethod>();

        static NBTTag()
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.Public;

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var attrib = (TagTypeAttribute)type.GetCustomAttributes(typeof(TagTypeAttribute), false).FirstOrDefault();
                if (attrib == null) continue;

                var baseType = type;
                MethodInfo readMethod = null;

                while (baseType != null && readMethod == null)
                {
                    var methods = baseType.GetMethods(flags);
                    readMethod = methods
                        .Where(x => x.Name == "Read")
                        .Where(x => typeof(NBTTag).IsAssignableFrom(x.ReturnType))
                        .Where(x => x.GetParameters().Length == 1)
                        .Where(x => x.GetParameters()[0].ParameterType == typeof(BinaryReader))
                        .FirstOrDefault();
                    baseType = baseType.BaseType;
                }

                if (readMethod == null)
                {
                    Debug.WriteLine($"Unable to find a Read method for type '{type}'.");
                    continue;
                }

                var deleg = (ReadMethod)Delegate.CreateDelegate(typeof(ReadMethod), readMethod);

                foreach (var tag in attrib.Types)
                {
                    _sReadMethods.Add(tag, deleg);
                }
            }
        }

        public static NBTTag ReadTag(BinaryReader reader, bool readName = true, TAG type = TAG.None)
        {
            if (type == TAG.None) type = (TAG)reader.ReadByte();

            ReadMethod readMethod;
            if (!_sReadMethods.TryGetValue(type, out readMethod))
            {
                throw new Exception($"Unknown tag type '{type}'.");
            }

            var name = "";

            if (type != TAG.End && readName)
            {
                var nameLength = BigEndianHelper.ReadBigEndian<ushort>(reader);
                name = new string(reader.ReadChars(nameLength));
            }

            var tag = readMethod(reader);
            tag.name = name;
            tag.type = type;
            return tag;
        }

        public static explicit operator byte(NBTTag tag)
        {
            return tag.GetConvertedValue<byte>();
        }

        public static explicit operator short(NBTTag tag)
        {
            return tag.GetConvertedValue<short>();
        }

        public static explicit operator int(NBTTag tag)
        {
            return tag.GetConvertedValue<int>();
        }

        public static explicit operator long(NBTTag tag)
        {
            return tag.GetConvertedValue<long>();
        }

        public static explicit operator float(NBTTag tag)
        {
            return tag.GetConvertedValue<float>();
        }

        public static explicit operator string(NBTTag tag)
        {
            return tag.GetConvertedValue<string>();
        }

        public static explicit operator double(NBTTag tag)
        {
            return tag.GetConvertedValue<double>();
        }

        public static explicit operator byte[] (NBTTag tag)
        {
            return ((NBTByteArrayTag)tag).value;
        }

        public TAG type;
        public string name;

        public virtual NBTTag this[string name]
        {
            get { return null; }
        }

        public virtual NBTTag this[int index]
        {
            get { return null; }
        }

        public override string ToString()
        {
            return type.ToString();
        }

        internal virtual K GetConvertedValue<K>()
            where K : IConvertible
        {
            throw new NotImplementedException();
        }
    }

    public class NBTTag<T> : NBTTag
    {
        public T value;

        public override string ToString()
        {
            return string.IsNullOrEmpty(name) ? $"{value}" : $"{name}: {value}";
        }
    }

    public class NBTStructTag<T> : NBTTag<T>
        where T : struct, IConvertible
    {
        public static NBTTag Read(BinaryReader reader)
        {
            return new NBTStructTag<T>
            {
                value = BigEndianHelper.ReadBigEndian<T>(reader)
            };
        }

        internal override K GetConvertedValue<K>()
        {
            return (K)Convert.ChangeType(value, typeof(K));
        }
    }


    [TagType(TAG.End)]
    public class NBTEndTag : NBTTag
    {
        public static NBTTag Read(BinaryReader reader)
        {
            return new NBTEndTag();
        }
    }

    [TagType(TAG.Byte)]
    public class NBTByteTag : NBTStructTag<byte> { }

    [TagType(TAG.Short)]
    public class NBTShortTag : NBTStructTag<short> { }

    [TagType(TAG.Int)]
    public class NBTIntTag : NBTStructTag<int> { }

    [TagType(TAG.Long)]
    public class NBTLongTag : NBTStructTag<long> { }

    [TagType(TAG.Float)]
    public class NBTFloatTag : NBTStructTag<float> { }

    [TagType(TAG.Double)]
    public class NBTDoubleTag : NBTStructTag<double> { }

    [TagType(TAG.Byte_Array)]
    public class NBTByteArrayTag : NBTTag<byte[]>
    {
        public static NBTTag Read(BinaryReader reader)
        {
            int length = BigEndianHelper.ReadBigEndian<int>(reader);
            return new NBTByteArrayTag
            {
                value = reader.ReadBytes(length)
            };
        }

        public override string ToString()
        {
            return $"ByteArray [{value.Length}]";
        }
    }

    [TagType(TAG.String)]
    public class NBTStringTag : NBTTag<string>
    {
        public static NBTTag Read(BinaryReader reader)
        {
            int length = BigEndianHelper.ReadBigEndian<short>(reader);

            return new NBTStringTag
            {
                value = System.Text.Encoding.UTF8.GetString(reader.ReadBytes(length))
            };
        }

        internal override K GetConvertedValue<K>()
        {
            return (K)Convert.ChangeType(value, typeof(K));
        }
    }

    [TagType(TAG.List)]
    public class NBTListTag : NBTTag<List<NBTTag>>
    {
        public static NBTTag Read(BinaryReader reader)
        {
            var list = new List<NBTTag>();
            var tagid = (TAG)reader.ReadByte();
            var length = BigEndianHelper.ReadBigEndian<int>(reader);
            for (var i = 0; i < length; i++)
            {
                var tag = ReadTag(reader, false, tagid);
                list.Add(tag);
            }
            return new NBTListTag
            {
                value = list
            };
        }

        public override NBTTag this[int index]
        {
            get
            {
                return value[index];
            }
        }

        public override string ToString()
        {
            return $"List [{value.Count}]";
        }
    }

    [TagType(TAG.Compound)]
    public class NBTCompoundTag : NBTTag<List<NBTTag>>
    {
        public static NBTTag Read(BinaryReader reader)
        {
            var list = new List<NBTTag>();

            while (true)
            {
                var tag = ReadTag(reader);
                if (tag.type == TAG.End) break;

                list.Add(tag);
            }

            return new NBTCompoundTag
            {
                value = list
            };
        }

        public override NBTTag this[int index]
        {
            get
            {
                return value[index];
            }
        }

        public override NBTTag this[string name]
        {
            get
            {
                return value.FirstOrDefault(x => x.name == name);
            }
        }

        public override string ToString()
        {
            return $"Compound [{value.Count}]";
        }
    }

    [TagType(TAG.Int_Array)]
    public class NBTIntArrayTag : NBTTag<int[]>
    {
        public static NBTTag Read(BinaryReader reader)
        {
            var list = new List<int>();
            var length = BigEndianHelper.ReadBigEndian<int>(reader);
            for (var i = 0; i < length; i++)
            {
                list.Add(BigEndianHelper.ReadBigEndian<int>(reader));
            }
            return new NBTIntArrayTag
            {
                value = list.ToArray()
            };
        }

        public override string ToString()
        {
            return $"IntArray [{value.Length}]";
        }
    }
}
