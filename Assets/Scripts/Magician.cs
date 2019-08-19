namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Magician : Enemy
    {
        [SerializeField]
        private Book book_left;
        [SerializeField]
        private Book book_center;
        [SerializeField]
        private Book book_right;
        private List<Book> bookList;
        public float hp;
        private int maxPattern;
        private float bookRotationOffset;
        private const float Slope_sin = 0.173648f;
        private const float Slope_cos = 0.494808f;
        private const float oneThirdPi = Mathf.PI / 3;
        public void Start()
        {
            bulletManager = FindObjectOfType<BulletManager>();
            bookList = new List<Book>();
            bookList.Add(book_left);
            bookList.Add(book_center);
            bookList.Add(book_right);
            foreach (Book book in bookList)
            {
                book.Initialize();
            }
            Reset();
        }
        public void Reset()
        {
            StopAllCoroutines();
            hp = 100;
            maxPattern = 1;
            bookRotationOffset = 0;
            ResetBookPosition();
            book_left.Reset();
            book_center.Reset();
            book_right.Reset();
            StartCoroutine(StopWatch());
        }
        public void ResetBookPosition()
        {
            float angle = bookRotationOffset;
            book_left.initialPosition = BookPosition(angle - 2 * oneThirdPi);
            book_center.initialPosition = BookPosition(angle + 2 * oneThirdPi);
            book_right.initialPosition = BookPosition(angle);
        }
        private Vector3 BookPosition(float angle)
        {
            float sinAngle = Mathf.Sin(angle);
            float cosAngle = Mathf.Cos(angle);
            return new Vector3(0.2f * cosAngle, sinAngle * Slope_sin, sinAngle * Slope_cos);
        }
        void Update()
        {
            maxPattern = (hp > 80) ? 1 : (hp > 30 ? 2 : 3);
            int currentPatternNum = (book_left.isIdle ? 0 : 1) + (book_center.isIdle ? 0 : 1) + (book_right.isIdle ? 0 : 1);
            if (currentPatternNum < maxPattern)
            {
                bookList[Random.Range(0, bookList.Count)].RandomPattern();
            }
            bookRotationOffset += 1 * Time.deltaTime;
            ResetBookPosition();
        }
        IEnumerator StopWatch()
        {
            while (hp > 0)
            {
                yield return new WaitForSeconds(1.0f);
                hp -= 1;
            }
        }
    }

}
