using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Core.BlocksCore
{
	public interface ISerializableBlockComponentContainer : IBlockComponentContainer
	{
		public string Serialize(Block block);
		public void InitializeBlock(Block block, string serializedData);
	}
}
