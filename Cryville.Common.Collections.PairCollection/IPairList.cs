using System.Collections;

namespace Cryville.Common.Collections {
	public interface IPairList : IList {
		void Add(object key, object value);
		void Insert(int index, object key, object value);
	}
}
