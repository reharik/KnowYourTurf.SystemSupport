using System;
using SystemSupport.Security.Interfaces;

namespace SystemSupport.Security.Model
{
	/// <summary>
	/// This is a trivial class that is used to make sure that Equals and GetHashCode
	/// are properly overloaded with the correct semantics. This is exteremely important
	/// if you are going to deal with objects outside the current Unit of Work.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class EqualityAndHashCodeProvider<T> : IIDentifiable
		where T : EqualityAndHashCodeProvider<T>
	{
		private int? oldHashCode;

	    /// <summary>
	    /// Gets or sets the id of this entity
	    /// </summary>
	    /// <value>The id.</value>
	    public virtual int EntityId { get; set; }

	    /// <summary>
		/// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
		/// <returns>
		/// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			T other = obj as T;
			if (other == null)
				return false;
			//to handle the case of comparing two new objects
			bool otherIsTransient = Equals(other.EntityId, Guid.Empty);
			bool thisIsTransient = Equals(EntityId, Guid.Empty);
			if (otherIsTransient && thisIsTransient)
				return ReferenceEquals(other, this);
			return other.EntityId.Equals(EntityId);
		}


		/// <summary>
		/// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>
		/// A hash code for the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override int GetHashCode()
		{
			//This is done se we won't change the hash code
			if (oldHashCode.HasValue)
				return oldHashCode.Value;

			bool thisIsTransient = Equals(EntityId, Guid.Empty);
			//When we are transient, we use the base GetHashCode()
			//and remember it, so an instance can't change its hash code.
			if (thisIsTransient)
			{
				oldHashCode = base.GetHashCode();
				return oldHashCode.Value;
			}
			return EntityId.GetHashCode();
		}


		/// <summary>
		/// Equality operator so we can have == semantics
		/// </summary>
		public static bool operator ==(EqualityAndHashCodeProvider<T> x, EqualityAndHashCodeProvider<T> y)
		{
			return Equals(x, y);
		}


		/// <summary>
		/// Inequality operator so we can have != semantics
		/// </summary>
		public static bool operator !=(EqualityAndHashCodeProvider<T> x, EqualityAndHashCodeProvider<T> y)
		{
			return !(x == y);
		}
	}
}