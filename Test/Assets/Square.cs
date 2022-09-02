using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    delegate bool queueEvent(float dt);
    queueEvent[] quEvent;
    int quIndex;
    void Start()
    {
        runA = 0.0f;
        runB = 0.0f;
        runC = 0.0f;
        runD = 0.0f;
        quEvent = new queueEvent[] { updateA, updateB, updateC, updateD };
        quIndex = 0;
    }



    void Update()
    {
        if (quIndex < quEvent.Length)// 대기 명령있을 때
		{
            if (quEvent[quIndex](Time.deltaTime))
			{
                quIndex++;
                if (quIndex == quEvent.Length)
                    ;// 모든 대기 명령 완료된 시점
            }
        }
    }

    float runA, runB, runC, runD;
    bool updateA(float dt)
	{
        // do something

        runA += dt;
        return !(runA < 3.0f);
	}
    bool updateB(float dt)
    {
        // do something

        runB += dt;
        return !(runB < 5f);
    }
    bool updateC(float dt)
    {
        // do something

        runC += dt;
        return !(runC < 4);
    }
    bool updateD(float dt)
    {
        // do something

        runD += dt;
        return !(runD < 2);
    }
}


