using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace hashes
{
    public class ReadonlyBytes : IEnumerable
    {
        private byte[] bytesArray;
        private int hashCode = -1;

        public ReadonlyBytes(params byte[] args)
        {
            bytesArray = args ?? throw new ArgumentNullException();
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= bytesArray.Length) throw new IndexOutOfRangeException();
                return bytesArray[index];
            }
        }

        public int Length => bytesArray.Length;

        public IEnumerator<byte> GetEnumerator()
        {
            return ((IEnumerable<byte>)bytesArray).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            if (!(obj is ReadonlyBytes secondBytes) || bytesArray.Length != secondBytes.Length)
                return false;
            return !bytesArray.Where((el, i) => el != secondBytes[i]).Any();
        }

        public override int GetHashCode()
        {
            /* if an expression produces a value that is outside the range of the destination type,
            the overflow is not flagged.*/
            unchecked
            {
                if (hashCode != -1) return hashCode;
                hashCode = 1;
                foreach (var item in bytesArray)
                {
                    hashCode *= 341;
                    hashCode += item * 2;
                }
                return hashCode;
            }
        }

        public override string ToString()
        {
            // joining the array elements with comma and putting them in square brackets as per tests
            return $"[{string.Join(", ", bytesArray)}]";
        }
    }
}