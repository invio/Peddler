using System;
using System.Collections.Generic;

namespace Peddler {

    public struct FakeStruct : IEquatable<FakeStruct>, IComparable<FakeStruct> {

        public int Value { get; set; }

        public override int GetHashCode() {
            return this.Value.GetHashCode();
        }

        public override bool Equals(Object that) {
            if (that is FakeStruct) {
                return this.Equals((FakeStruct)that);
            }

            return false;
        }

        public bool Equals(FakeStruct that) {
            return this.Value == that.Value;
        }

        public int CompareTo(FakeStruct other) {
            return this.Value.CompareTo(other.Value);
        }

        public override String ToString() {
            return $"{{ '{nameof(Value)}': {this.Value:N0} }}";
        }

    }

}
