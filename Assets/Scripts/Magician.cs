namespace BossShooter
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Magician : MonoBehaviour
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
        public void Initialize()
        {
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
            book_left.Reset(new Vector3(-0.15f, 0f, 0f));
            book_center.Reset(new Vector3(0.13f, -0.05f, 0f));
            book_right.Reset(new Vector3(0.2f, 0.06f, 0f));
            StartCoroutine(StopWatch());
        }
        void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.Q))
            {
                book_left.Pattern("Spread", BulletType.Random);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                book_left.Pattern("Fall");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                book_left.Pattern("Rush");
            }
            */
            maxPattern = (hp > 70) ? 1 : (hp > 30 ? 2 : 3);
            int currentPatternNum = (book_left.isIdle ? 0 : 1) + (book_center.isIdle ? 0 : 1) + (book_right.isIdle ? 0 : 1);
            if (currentPatternNum < maxPattern)
            {
                bookList[Random.Range(0, bookList.Count)].RandomPattern();
            }
            Debug.Log(maxPattern + "," + currentPatternNum);
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
