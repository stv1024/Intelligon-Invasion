using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuffer;
namespace Assets.Scripts.Data.Proto
{
	/// <summary>
	/// </summary>
	public class UserStageData : ISendable, IReceiveable
	{
		private bool HasId{get;set;}
		private int id;
		/// <summary>
		/// </summary>
		public int Id
		{
			get
			{
				return id;
			}
			set
			{
				HasId = true;
				id = value;
			}
		}

		private bool HasStars{get;set;}
		private int stars;
		/// <summary>
		/// 只要有数据，则已经解锁
		/// </summary>
		public int Stars
		{
			get
			{
				return stars;
			}
			set
			{
				HasStars = true;
				stars = value;
			}
		}

		private bool HasScore{get;set;}
		private int score;
		/// <summary>
		/// </summary>
		public int Score
		{
			get
			{
				return score;
			}
			set
			{
				HasScore = true;
				score = value;
			}
		}

		/// <summary>
		/// </summary>
		public UserStageData()
		{
		}

		/// <summary>
		/// </summary>
		public UserStageData
		(
			int id,
			int stars,
			int score
		):this()
		{
			Id = id;
			Stars = stars;
			Score = score;
		}
		public byte[] GetProtoBufferBytes()
		{
			ProtoBufferWriter writer = new ProtoBufferWriter();
			writer.Write(1,Id);
			writer.Write(2,Stars);
			writer.Write(3,Score);
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
						Id = obj.Value;
						break;
					case 2:
						Stars = obj.Value;
						break;
					case 3:
						Score = obj.Value;
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
			sb.Append("Id : " + Id + ",");
			sb.Append("Stars : " + Stars + ",");
			sb.Append("Score : " + Score);
			sb.Append("}");
			return sb.ToString();
		}
	}
}
