using UnityEngine;

public class Datas : Singleton<Datas>
{

    private TextAsset datas;


    public string[] getData()
    {
        datas = Resources.Load<TextAsset>("datas/datas");
        string[] lines = new string[0];
        lines = datas.text.Split('\n');

        return lines;
    }


}
