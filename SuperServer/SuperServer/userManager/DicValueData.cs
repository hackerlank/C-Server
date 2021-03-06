﻿using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using SuperProto;

namespace SuperServer.userManager
{


    [Serializable]
    public class DicValueData<T> : Dictionary<int, T>,IDicValueData where T : struct
    {
        private List<CHANGE_TYPE> typeList = new List<CHANGE_TYPE>();

        private List<int> indexList = new List<int>();

        private string name;
        
        public DicValueData()
        {

        }

        protected DicValueData(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            name = (string)info.GetValue("name", typeof(string));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name, typeof(string));

            base.GetObjectData(info, context);
        }

        public new T this[int _index]
        {
            get {

                return base[_index];
            }

            set {

                base[_index] = value;

                typeList.Add(CHANGE_TYPE.CHANGE);

                indexList.Add(_index);
            }
        }
        
        public new void Add(int _index,T _data)
        {
            base.Add(_index, _data);

            typeList.Add(CHANGE_TYPE.ADD);

            indexList.Add(_index);
        }

        public new void Remove(int _index)
        {
            base.Remove(_index);

            typeList.Add(CHANGE_TYPE.REMOVE);

            indexList.Add(_index);
        }

        public bool IsChange()
        {
            return typeList.Count > 0;
        }

        public void SetName(string _name)
        {
            name = _name;
        }

        public string GetName()
        {
            return name;
        }

        public DicBase GetData()
        {
            Dic<T> data = new Dic<T>();

            data.name = name;

            data.index = new int[Count];
            
            data.data = new T[Count];

            int i = 0;

            Dictionary<int, T>.Enumerator enumerator = GetEnumerator();

            while (enumerator.MoveNext())
            {
                data.index[i] = enumerator.Current.Key;

                data.data[i] = enumerator.Current.Value;

                i++;
            }
            
            if(indexList.Count > 0)
            {
                indexList.Clear();
                typeList.Clear();
            }
            
            return data;
        }

        public DicChangeBase GetChangeData()
        {
            DicChange<T> changeData = new DicChange<T>();

            changeData.name = name;

            changeData.index = new int[indexList.Count];

            changeData.type = new CHANGE_TYPE[indexList.Count];

            changeData.data = new T[indexList.Count];

            for (int i = 0; i < indexList.Count; i++)
            {
                int index = indexList[i];

                changeData.index[i] = index;

                CHANGE_TYPE type = typeList[i];

                changeData.type[i] = type;

                if(type != CHANGE_TYPE.REMOVE)
                {
                    changeData.data[i] = this[index];
                }
            }

            indexList.Clear();

            typeList.Clear();

            return changeData;
        }
    }
}
