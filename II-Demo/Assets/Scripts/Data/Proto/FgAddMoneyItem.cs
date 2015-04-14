using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class FgAddMoneyItem : ISendable, IReceiveable
	{
		private bool HasMoneyPerSecond{get;set;}
		private float moneyPerSecond;
		/// <summary>
		/// </summary>
		public float MoneyPerSecond
		{
			get
			{
				return moneyPerSecond;
			}
			set
			{
				HasMoneyPerSecond = true;
				moneyPerSecond = value;
			}
		}

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
		public FgAddMoneyItem()
		{
		}

		/// <summary>
		/// </summary>
		public FgAddMoneyItem
		(
			float moneyPerSecond,
			float initCost
		):this()
		{
			MoneyPerSecond = moneyPerSecond;
			InitCost = initCost;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,MoneyPerSecond);
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
					case 1:
						MoneyPerSecond = obj.Value;
						break;
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
			sb.Append("MoneyPerSecond : " + MoneyPerSecond + ",");
			sb.Append("InitCost : " + InitCost);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
