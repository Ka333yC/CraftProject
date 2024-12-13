﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Core.BlocksCore
{
	public interface ISerializedBlockComponent : IBlockComponent
	{
		public string Serialize();
		public void Populate(string serializedData);
	}
}
