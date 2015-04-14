using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class SwipeAddMoneyItem : ISendable, IReceiveable
	{
		private bool HasInitCost{get;set;}
		private float initCost;
		/// <summary>
		/// </summary>
		public float InitCost
		{
			get
			{
				return initCost;
			}
			set
			{
				HasInitCost = true;
				initCost = value;
			}
		}

		/// <summary>
		/// </summary>
		public SwipeAddMoneyItem()
		{
		}

		/// <summary>
		/// </summary>
		public SwipeAddMoneyItem
		(
			float initCost
		):this()
		{
			InitCost = initCost;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(2,InitCost);
			return writer.GetProtoBufferBytes();
		}
		public void ParseFrom(byte[] buffer)
		{
			 ParseFrom(buffer, 0, buffer.Length);
		}
		public void ParseFrom(byte[] buffer, int offset, int size)
		{
			if (buffer == null) return;
			ProtoBufferReader reader = new ProtoBufferReader(buffer,offset,size);
			foreach (ProtoBufferObject obj in reader.ProtoBufferObjs)
			{
				switch (obj.FieldNumber)
				{
					case 2:
						InitCost = obj.Value;
						break;
					default:
						break;
				}
			}
		}
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("{");
			sb.Append("InitCost : " + InitCost);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
