using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_physics : MonoBehaviour {
    public GameObject PauseMenu, UI, Stats;
    public GameObject Circle;
    public bool end = false;
    private Animator animator;
    public string Horizontal_Axis;
    public string Vertical_Axis;
    private bool CanMove = true;
    public bool Movement = true;
    private void Start()
    {
        animator = new Animator();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void FixedUpdate()
     {
        //  float prevX = transform.position.x;
        //  float prevZ = transform.position.z;
        var x = 0f;
        var z = 0f;
        if (Movement)
        {
            x = Input.GetAxis(Horizontal_Axis) * Time.deltaTime * gameObject.GetComponent<Additional_power_ups>().speed;
            z = Input.GetAxis(Vertical_Axis) * Time.deltaTime * gameObject.GetComponent<Additional_power_ups>().speed;
            transform.Translate(x, 0, z);
        }
        else
        {
            float posX;
            float posZ;
            if (CanMove)
            {
                CenterPosition(this.gameObject, out posX, out posZ);
                this.gameObject.transform.localPosition = new Vector3(posX, transform.localPosition.y, posZ);
            }
            x = (float)Input.GetAxis(Horizontal_Axis);
            z = (float)Input.GetAxis(Vertical_Axis);
            if(Mathf.Abs(x)>Mathf.Abs(z))
            {
                z = 0;
                if(x>0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                }
            }
            else if(Mathf.Abs(x)<Mathf.Abs(z))
            {
                x = 0;
                if (z > 0)
                {
                    z = 1;
                }
                else
                {
                    z = -1;
                }
            }

            if (CanMove&&(Mathf.Abs(x)>0||Mathf.Abs(z)>0) && AbleToMove(x, z))
            {
                CanMove = false;
                //transform.Translate(x, 0, z);
                Vector3 target = this.transform.localPosition + new Vector3(x, 0, z);
                StartCoroutine(MoveToPosition(this.transform, target, 0.1f));
            }
        }
        animator = this.gameObject.GetComponent<Animator>();
        if (x != 0 || z != 0)
        {
            animator.SetTrigger("Walking");
            animator.ResetTrigger("Standing");
        }
        else
        {
            animator.SetTrigger("Standing");
            animator.ResetTrigger("Walking");
        }

        if (Input.GetButton("Cancel")&&(!PauseMenu.activeInHierarchy)&&!end)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UI.SetActive(false);
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }   
        if(Input.GetButton("Stats")&&(!Stats.activeInHierarchy))
        {
            Stats.SetActive(true);
            UI.SetActive(false);
        }
        else if(!Input.GetButton("Stats"))
        {
            Stats.SetActive(false);
            UI.SetActive(true);
        }
     }
    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1f)
        {
            Circle.SetActive(true);
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            Circle.transform.localScale = Vector3.Lerp(Circle.transform.localScale, new Vector3(0.6f, 0.6f, 1f), t);
            yield return null;
        }
        Circle.SetActive(false);
        Circle.transform.localScale = new Vector3(1f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        CanMove = true;
    }
    private bool AbleToMove(float x, float z)
    {
        float angle = this.transform.localRotation.eulerAngles.y;
        angle = Mathf.Round(angle / 90) * 90;
        if (angle == 360)
        {
            angle = 0;
        }
        switch (angle.ToString())
        {
            case "0":
                break;
            case "90":
                var c = x;
                x = z;
                z = -c;
                break;
            case "180":
                x = -x;
                z = -z;
                break;
            case "270":
                var d = x;
                x = -z;
                z = d;
                break;
        }
        int i = -1;
        if (z == 1)
        {
            i = 0;
        }
        else if (z == -1)
        {
            i = 2;
        }
        else if (x == -1)
        {
            i = 3;
        }
        else if(x==1)
        {
            i = 1;
        }
        if (i != -1)
        {
            RaycastHit Hit;
            int layer_mask = LayerMask.GetMask("Player", "Map", "Bombs", "AI");
            if (Physics.SphereCast(transform.position + new Vector3(0f, 0.5f, 0f), 0.48f, Quaternion.Euler(0, i * 90, 0) * Vector3.forward, out Hit, Mathf.Infinity, layer_mask))
            {
                float distance1 = Hit.distance;
                if((Hit.distance<1f)&&(this.gameObject.GetComponent<Additional_power_ups>().bomb_kick)&&(Hit.collider.gameObject.layer==11))
                {
                    Kick(Hit.collider.gameObject, Quaternion.Euler(0, i * 90, 0) * Vector3.forward);
                    return true;
                }
                else if(Hit.distance < 1f)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private void CenterPosition(GameObject obj, out float posX, out float posZ)
    {
        Vector3 pos = new Vector3();
        pos = obj.GetComponent<Transform>().localPosition;
        if (pos.x >= 0)
        {
            posX = (int)pos.x + 0.5f;
        }
        else
        {
            posX = Mathf.Sign(pos.x) * (Mathf.Abs((int)pos.x) + 0.5f);
        }
        if (pos.z >= 0)
        {
            posZ = (int)pos.z + 0.5f;
        }
        else
        {
            posZ = Mathf.Sign(pos.z) * ((int)Mathf.Abs(pos.z) + 0.5f);
        }
    }
    private void Kick(GameObject bomb, Vector3 direction)
    {
                bomb.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                bomb.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
                bomb.GetComponent<SphereCollider>().material = null;
                bomb.GetComponent<Bomb_spawn_collision>().kicked = true;
    }
}
