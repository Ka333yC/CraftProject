using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Core.BlocksCore
{
	public interface ISerializableBlockContainer : IBlockContainer
	{
		public string Serialize(Block block);
		public Block Deserialize(string serializedBlock);
	}
}
