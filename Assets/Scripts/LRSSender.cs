using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using System;
using TMPro;

public class LRSSender : MonoBehaviour
{

    RemoteLRS lrs;
    public TextMeshProUGUI logger;
    // Start is called before the first frame update
    void Start()
    {
        // try {
        // lrs = new RemoteLRS(
        // "https://tester-coulombs.lrs.io/xapi/",
        // "d1234",
        // "p1234"
        // );
        // }
        // catch (Exception e)
        // {
        //     logger.text = e.Message;
        // }
    }

    public void SendLRS(string _actor, string _verb, string _definition, int _value)
    {
        // Agent actor = new Agent();
        // actor.mbox = "mailto:" + _actor.Replace(" ", "") + "@email.com";
        // actor.name = _actor;

        // //Build out Verb details
        // Verb verb = new Verb();
        // verb.id = new Uri("http://www.example.com/" + _verb.Replace(" ", ""));
        // verb.display = new LanguageMap();
        // verb.display.Add("en-US", _verb);

        // //Build out Activity details
        // Activity activity = new Activity();
        // activity.id = new Uri("http://www.example.com/" + _definition.Replace(" ", "")).ToString();

        // //Build out Activity Definition details
        // ActivityDefinition activityDefinition = new ActivityDefinition();
        // activityDefinition.description = new LanguageMap();
        // activityDefinition.name = new LanguageMap();
        // activityDefinition.name.Add("en-US", (_definition));
        // activity.definition = activityDefinition;

        // Result result = new Result();
        // Score score = new Score();

        // score.raw = _value;
        // result.score = score;

        // //Build out full Statement details
        // Statement statement = new Statement();
        // statement.actor = actor;
        // statement.verb = verb;
        // statement.target = activity;
        // statement.result = result;

        // //Send the statement
        // StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);
        // if (lrsResponse.success) //Success
        // {
        //     logger.text = "Save statement: " + lrsResponse.content.id;
        // }
        // else //Failure
        // {
        //     logger.text = "Statement Failed: " + lrsResponse.errMsg;
        // }
    }
}
