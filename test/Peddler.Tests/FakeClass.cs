using System;
using System.Collections.Generic;

namespace Peddler {

    public class FakeClass : IEquatable<FakeClass>, IComparable<FakeClass> {

        public int Value { get; }

        public FakeClass(int value) {
            this.Value = value;
        }

        public override int GetHashCode() {
            return this.Value.GetHashCode();
        }

        public override bool Equals(Object that) {
            return this.Equals(that as FakeClass);
        }

        public bool Equals(FakeClass that) {
            return that != null
                && this.Value == that.Value;
        }

        public int CompareTo(FakeClass other) {
            if (other == null) {
                return 1;
            }

            return this.Value.CompareTo(other.Value);
        }

        public override String ToString() {
            return $"{{ '{nameof(Value)}': {this.Value:N0} }}";
        }

    }

}
