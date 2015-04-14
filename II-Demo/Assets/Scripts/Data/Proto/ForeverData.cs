using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class ForeverData : ISendable, IReceiveable
	{
		private bool HasMoney{get;set;}
		private long money;
		/// <summary>
		/// </summary>
		public long Money
		{
			get
			{
				return money;
			}
			set
			{
				HasMoney = true;
				money = value;
			}
		}

		private List<UserStageData> userStageDataList;
		/// <summary>
		/// 用户关卡数据。无数据的stage表示锁定。
		/// </summary>
		public List<UserStageData> UserStageDataList
		{
			get
			{
				return userStageDataList;
			}
			set
			{
				if(value != null)
				{
					userStageDataList = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		public ForeverData()
		{
			UserStageDataList = new List<UserStageData>();
		}

		/// <summary>
		/// </summary>
		public ForeverData
		(
			long money
		):this()
		{
			Money = money;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Money);
			foreach(UserStageData v in UserStageDataList)
			{
				writer.Write(2,v);
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
						Money = obj.Value;
						break;
					case 2:
						 var userStageData= new UserStageData();
						userStageData.ParseFrom(obj.Value);
						UserStageDataList.Add(userStageData);
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
			sb.Append("Money : " + Money + ",");
			sb.Append("UserStageDataList : [");
			for(int i = 0; i < UserStageDataList.Count;i ++)
			{
				if(i == UserStageDataList.Count -1)
				{
					sb.Append(UserStageDataList[i]);
				}else
				{
					sb.Append(UserStageDataList[i] + ",");
				}
			}
			sb.Append("]");
			sb.Append("}");
			return sb.ToString();
		}
	}
}
