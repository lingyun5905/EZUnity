/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-04-12 10:14:27
 * Organization:    SmileTech
 * Description:     
 */
using System;
using UnityEngine;

namespace EZUnity.Playables
{
    public interface IKeyframe
    {
        float time { get; set; }
    }

    public interface IKeyframe<T> : IKeyframe
    {
        T value { get; set; }
    }

    [Serializable]
    public struct FloatKeyframe : IKeyframe<float>
    {
        [SerializeField]
        private float m_Time;
        public float time { get { return m_Time; } set { m_Time = value; } }

        [SerializeField]
        private float m_Value;
        public float value { get { return m_Value; } set { m_Value = value; } }
    }

    [Serializable]
    public struct Vector3Keyframe : IKeyframe<Vector3>
    {
        [SerializeField]
        private float m_Time;
        public float time { get { return m_Time; } set { m_Time = value; } }

        [SerializeField]
        private Vector3 m_Value;
        public Vector3 value { get { return m_Value; } set { m_Value = value; } }
    }
}
