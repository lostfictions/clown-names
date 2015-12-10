using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class ClownNameGenerator : MonoBehaviour
{
    public TextAsset clownNames;

    Text text;

    string[] splitNames;

    void Awake()
    {
        splitNames = clownNames.text.Split('\n');

        text = GetComponent<Text>();
        text.MustNotBeNull();

        SetName();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            SetName();
        }
    }

    void SetName()
    {
        var ct = Random.Range(2, 5);
        var ns = new List<string>();
        for(int i = 0; i < ct; i++) {
            ns.Add(splitNames[Random.Range(0, splitNames.Length)]);
        }

        string clownName = ns
            .Select(n => Syllabize(n))
            .Select(syllableGroup => syllableGroup.ToArray())
            .Aggregate("", (current, sg) => current + sg[Random.Range(0, sg.Length)]);

        //Capitalize first letter
        clownName = char.ToUpper(clownName[0]) + clownName.Substring(1);

        text.text = clownName;
    }


    static readonly char[] vowels = {'a', 'e', 'i', 'o', 'u', 'y'};
    static List<string> Syllabize(string word)
    {
        var syllables = new List<string>();

        int lastSplitIndex = 0;

        for(int i = 0; i < word.Length - 1; i++) {
            if(!vowels.Contains(word[i]) && i > 1 && vowels.Contains(word[i - 1])) {
                syllables.Add(word.Substring(lastSplitIndex, i - lastSplitIndex + 1));
                lastSplitIndex = i + 1;
            }
        }

        syllables.Add(word.Substring(lastSplitIndex, word.Length - lastSplitIndex));

        return syllables;
    }
}
