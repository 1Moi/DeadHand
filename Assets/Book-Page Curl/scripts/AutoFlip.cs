using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RawBook))]
public class AutoFlip : MonoBehaviour
{
    public FlipMode Mode;
    public float PageFlipTime = 1;
    public float TimeBetweenPages = 1;
    public float DelayBeforeStarting = 0;
    public bool AutoStartFlip = true;
    public int AnimationFramesCount = 40;
    private bool IsFlipToEnd = false;

    bool isFlipping = false;

    [SerializeField] private RawBook ControledBook;
    [SerializeField] private GameObject MangeClick;

    void Start()
    {
        if (!ControledBook)
            ControledBook = GetComponent<RawBook>();
        if (AutoStartFlip)
            StartFlipping();
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));
    }

    void PageFlipped()
    {
        isFlipping = false;
        if (IsFlipToEnd == false)
        {
            MangeClick.SetActive(false);
        }        
    }

    public void StartFlipping()
    {
        StartCoroutine(FlipToEnd());
    }

    public void FlipRightPage()
    {
        if (isFlipping) return;
        if (ControledBook.currentPage >= ControledBook.TotalPageCount - 1) return;

        isFlipping = true;
        MangeClick.SetActive(true);

        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;

        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
    }

    public void FlipLeftPage()
    {
        if (isFlipping) return;
        if (ControledBook.currentPage <= 1) return;

        isFlipping = true;
        MangeClick.SetActive(true);

        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;

        StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
    }

    public IEnumerator FlipToEnd()
    {
        IsFlipToEnd = true;
        yield return new WaitForSeconds(DelayBeforeStarting);

        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;

        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.currentPage < ControledBook.TotalPageCount - 3)
                {
                    if (isFlipping)
                    {
                        yield return null;
                        continue;
                    }
                    isFlipping = true;
                    MangeClick.SetActive(true);
                    yield return StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                    
                    isFlipping = false;
                    yield return new WaitForSeconds(TimeBetweenPages);
                }                
                IsFlipToEnd = false;
                yield break;
                

            case FlipMode.LeftToRight:
                while (ControledBook.currentPage > 1)
                {
                    if (isFlipping)
                    {
                        yield return null;
                        continue;
                    }
                    isFlipping = true;
                    MangeClick.SetActive(true);
                    yield return StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                    MangeClick.SetActive(false);
                    isFlipping = false;
                    yield return new WaitForSeconds(TimeBetweenPages);
                }
                yield break;
        }
    }


    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            // Mise à jour de la position de la page à chaque frame
            ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));

            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }
        ControledBook.ReleasePage();
    }

    IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc - xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            // Mise à jour de la position de la page à chaque frame
            ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
            ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));

            yield return new WaitForSeconds(frameTime);
            x += dx;
        }
        ControledBook.ReleasePage();
    }
}
