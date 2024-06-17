using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComicManager : MonoBehaviour
{
    [SerializeField] private PlayerInputReader reader;

    private Dictionary<GameObject, List<GameObject>> comicList = new Dictionary<GameObject, List<GameObject>>();
    private int pageCounter = 0;
    private int panelInPageCounter = 0;
    private string pageTag = "ComicPage";
    private string panelTag = "ComicPanel";
    private string doneParameterName = "isDone";

    private void OnEnable()
    {
        reader.jumpEventPerformed += OnJumpButtonPressed;
    }

    private void OnDisable()
    {
        reader.jumpEventPerformed -= OnJumpButtonPressed;
    }

    private void Start()
    {
        InitializeComicList();
    }

    private void InitializeComicList()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject pageChild = transform.GetChild(i).gameObject;

            if (pageChild.tag != pageTag)
                continue;

            List<GameObject> panelList = new List<GameObject>(); //Debug.Log(pageChild.name);

            for (int j = 0; j < pageChild.transform.childCount; j++)
            {
                GameObject panelChild = pageChild.transform.GetChild(j).gameObject;

                if (panelChild.tag != panelTag)
                    continue;

                panelList.Add(panelChild); //Debug.Log(panelChild.name);
                panelChild.transform.localScale = new Vector3(0, 0, 0);
            }

            comicList.Add(pageChild, panelList);
            pageChild.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    private void OnJumpButtonPressed()
    {
        ActivateNextPanel();
    }

    private void ActivateNextPanel()
    {
        if (pageCounter > comicList.Count - 1)
        {
            Debug.Log("COMIC OVER");
            return;
        }

        List<GameObject> panelsOfPage = comicList.ElementAt(pageCounter).Value;
        GameObject page = comicList.ElementAt(pageCounter).Key.gameObject;
        page.transform.localScale = new Vector3(1, 1, 1);

        if (panelInPageCounter > panelsOfPage.Count - 1)
        {
            //Go to next page
            page.transform.localScale = new Vector3(0, 0, 0);
            pageCounter++;
            panelInPageCounter = 0;
            ActivateNextPanel();
        }
        else
        {
            //Go to next panel
            panelsOfPage[panelInPageCounter].transform.localScale = new Vector3(1, 1, 1);
            if (panelInPageCounter > 0)
            {
                Animator animator = panelsOfPage[panelInPageCounter - 1].GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetBool(doneParameterName, true);
                }
            }
            panelInPageCounter++;
        }
    }
}

            loadingScreenHandler = Instantiate(loadingScreenPrefab).GetComponent<LoadingScreenHandler>();
            loadingScreenHandler.targetScene = sceneName;
            loadingScreenHandler.ToScene();