using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Leopotam.EcsLite
{
    public class EntityDestroyedException : Exception
    {
		public EntityDestroyedException() : base() { }
		public EntityDestroyedException(string message) : base(message) { }
		public EntityDestroyedException(string message, Exception innerException) : base(message, innerException) { }
		protected EntityDestroyedException(SerializationInfo info, StreamingContext context) : 
			base(info, context) { }
	}

	public class EmptyEntityException : Exception
	{
		public EmptyEntityException() : base() { }
		public EmptyEntityException(string message) : base(message) { }
		public EmptyEntityException(string message, Exception innerException) : base(message, innerException) { }
		protected EmptyEntityException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{ }
	}

	public class WorldDestroyedException : Exception
	{
		public WorldDestroyedException() : base() { }
		public WorldDestroyedException(string message) : base(message) { }
		public WorldDestroyedException(string message, Exception innerException) : base(message, innerException) { }
		protected WorldDestroyedException(SerializationInfo info, StreamingContext context) :
			base(info, context)
		{ }
	}
}
