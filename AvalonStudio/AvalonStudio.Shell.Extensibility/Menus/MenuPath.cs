using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Menus
{
    public class MenuPath : IEquatable<MenuPath>, IReadOnlyList<string>
    {
        public int Count => _path.Count;

        public MenuPath Parent
        {
            get
            {
                if (_parent != null)
                {
                    return _parent;
                }

                if (_path.Count == 0)
                {
                    return this;
                }

                return _parent = new MenuPath(_path.Take(_path.Count - 1).ToList());
            }
        }

        private IReadOnlyList<string> _path;
        private MenuPath _parent;

        public MenuPath(IReadOnlyList<string> path)
        {
            _path = path;
        }

        public string this[int index] => _path[index];

        public bool Equals(MenuPath other) => ReferenceEquals(this, other)
            || _path.SequenceEqual(other._path, StringComparer.OrdinalIgnoreCase);

        public override bool Equals(object obj) => obj is MenuPath menuPath && Equals(menuPath);
        public override int GetHashCode() => _path.Count == 0 ? 0 : _path[_path.Count - 1].GetHashCode();

        public IEnumerator<string> GetEnumerator() => _path.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static bool operator ==(MenuPath a, MenuPath b) => Equals(a, b);
        public static bool operator !=(MenuPath a, MenuPath b) => !(a == b);
    }
}
