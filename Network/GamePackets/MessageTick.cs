using KinSocket;
using System;
public class MessageTick
{
    private byte[] mData;

    public MessageTick()
    {
        this.mData = new byte[30 + 8];
        PacketConstructor.Write(30, 0, this.mData);
        PacketConstructor.Write((ushort)0x3f4, 2, this.mData);
    }

    public MessageTick(byte[] d)
    {
        this.mData = new byte[d.Length];
        d.CopyTo(this.mData, 0);
    }

    public static implicit operator byte[](MessageTick d)
    {
        return d.mData;
    }

    public uint Identifier
    {
        get
        {
            return BitConverter.ToUInt32(this.mData, 4);
        }
        set
        {
            PacketConstructor.Write(value, 4, this.mData);
        }
    }

    public uint Response
    {
        get
        {
            return BitConverter.ToUInt32(this.mData, 8);
        }
        set
        {
            PacketConstructor.Write(value, 8, this.mData);
        }
    }
}