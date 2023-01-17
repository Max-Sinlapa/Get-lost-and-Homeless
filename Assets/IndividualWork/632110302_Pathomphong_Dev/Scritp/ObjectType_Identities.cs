using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Max_DEV;

namespace Max_DEV
{
    public class ObjectType_Identities : MonoBehaviour
    {
        [SerializeField] 
        protected ObjectType m_ObjectType;
        public ObjectType Type
        {
            get { return m_ObjectType; }
            set { m_ObjectType = value; }
        }
    }

}
