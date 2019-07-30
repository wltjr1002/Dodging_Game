namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public enum BulletType
    {
        Linear,
        Spiral,
        Random,
        Homing,
    };

    public class Bullet : MonoBehaviour
    {
        public BulletPool bulletPool;
        public float width;
        public float height;
        private BulletType _bulletType;
        private Vector3 _Pinit;
        private Vector3 _Pnow;
        private Vector3 _Vinit;
        private Vector3 _Vnow;
        private float _speed;
        private float _curl;

        public void Initialize(BulletType type, Vector3 Position, Vector3 direction, float speed, float curl = 0.7f)
        {
            _bulletType = type;
            _Pinit = Position;
            _Vinit = Vector3.Normalize(direction);
            if(type == BulletType.Random)
            {
                float theta = Random.Range(0, Mathf.PI * 2);
                _Vinit = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            }
            _speed = speed;
            _curl = curl;
        }

        void Update()
        {
            // 방향벡터 계산
            switch (_bulletType)
            {
                case BulletType.Linear:
                case BulletType.Homing:
                case BulletType.Random:
                    {
                        _Vnow = _Vinit;
                        break;
                    }
                case BulletType.Spiral:
                    {
                        _Pnow = transform.localPosition;
                        Vector3 vtemp = (_Pnow-_Pinit);
                        _Vnow = Vector3.Normalize(new Vector3(-vtemp.y,vtemp.x,0)) + Vector3.Normalize(vtemp);
                        break;
                    }
                default: break;
            }
            transform.localPosition += _Vnow * (_speed * Time.deltaTime);

            // 총알이 밖으로 나가면 삭제
            Vector3 position = transform.localPosition;
            if (Mathf.Abs(position.x) > 5 || Mathf.Abs(position.y) > 10)
            {
                try
                {
                    bulletPool.GetBackObject(this);
                }
                catch (System.Exception)
                {
                    Debug.Log(gameObject.transform.localPosition);
                    throw;
                }
                return;
            }
        }

        
    }
}