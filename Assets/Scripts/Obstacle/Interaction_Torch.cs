using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_Torch : InteractionBase
{

    public LightController LightController;

    protected override void Start()
    {
        base.Start();
        btn.onClick.AddListener(Interaction);
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance < 2f)
        {
            interactionUI.SetActive(true);
        }else
        {
            interactionUI.SetActive(false);
        }
    }

    void Interaction()
    {
        if (LightController.onLight)
        {
            LightController.onLight = false;
        }
        else
        {
            LightController.onLight = true;
            LightController.LigtOn();
        }

    }


}
