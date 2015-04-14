using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class NumericData : ISendable, IReceiveable
	{
		private bool HasFgAddMoneyCostIncreaseFactor{get;set;}
		private float fgAddMoneyCostIncreaseFactor;
		/// <summary>
		/// 前台加钱项目每级开销增长
		/// </summary>
		public float FgAddMoneyCostIncreaseFactor
		{
			get
			{
				return fgAddMoneyCostIncreaseFactor;
			}
			set
			{
				HasFgAddMoneyCostIncreaseFactor = true;
				fgAddMoneyCostIncreaseFactor = value;
			}
		}

		private List<FgAddMoneyItem> fgAddMoneyList;
		/// <summary>
		/// 前台加钱项目
		/// </summary>
		public List<FgAddMoneyItem> FgAddMoneyList
		{
			get
			{
				return fgAddMoneyList;
			}
			set
			{
				if(value != null)
				{
					fgAddMoneyList = value;
				}
			}
		}

		private List<SwipeAddMoneyItem> swipeAddMoneyList;
		/// <summary>
		/// 划动加钱项目等级上限是3n-1
		/// </summary>
		public List<SwipeAddMoneyItem> SwipeAddMoneyList
		{
			get
			{
				return swipeAddMoneyList;
			}
			set
			{
				if(value != null)
				{
					swipeAddMoneyList = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public NumericData()
		{
			FgAddMoneyList = new List<FgAddMoneyItem>();
			SwipeAddMoneyList = new List<SwipeAddMoneyItem>();
		}

		/// <summary>
		/// </summary>
		public NumericData
		(
			float fgAddMoneyCostIncreaseFactor
		):this()
		{
			FgAddMoneyCostIncreaseFactor = fgAddMoneyCostIncreaseFactor;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,FgAddMoneyCostIncreaseFactor);
			foreach(FgAddMoneyItem v in FgAddMoneyList)
			{
				writer.Write(2,v);
			}
			foreach(SwipeAddMoneyItem v in SwipeAddMoneyList)
			{
				writer.Write(3,v);
			}
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
						FgAddMoneyCostIncreaseFactor = obj.Value;
						break;
					case 2:
						 var fgAddMoney= new FgAddMoneyItem();
						fgAddMoney.ParseFrom(obj.Value);
						FgAddMoneyList.Add(fgAddMoney);
						break;
					case 3:
						 var swipeAddMoney= new SwipeAddMoneyItem();
						swipeAddMoney.ParseFrom(obj.Value);
						SwipeAddMoneyList.Add(swipeAddMoney);
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
			sb.Append("FgAddMoneyCostIncreaseFactor : " + FgAddMoneyCostIncreaseFactor + ",");
			sb.Append("FgAddMoneyList : [");
			for(int i = 0; i < FgAddMoneyList.Count;i ++)
			{
				if(i == FgAddMoneyList.Count -1)
				{
					sb.Append(FgAddMoneyList[i]);
				}else
				{
					sb.Append(FgAddMoneyList[i] + ",");
				}
			}
			sb.Append("],");
			sb.Append("SwipeAddMoneyList : [");
			for(int i = 0; i < SwipeAddMoneyList.Count;i ++)
			{
				if(i == SwipeAddMoneyList.Count -1)
				{
					sb.Append(SwipeAddMoneyList[i]);
				}else
				{
					sb.Append(SwipeAddMoneyList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
