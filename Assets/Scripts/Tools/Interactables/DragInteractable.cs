/*
 *  Author: James Greensill
 *  Date: 30/10/2021
 *  Folder Location: Assets/Tools/Interactables
 *  
 *  Refactor: Ronan Richardson
 *  Date: 17/11/2021 - fixed events to actually fire, fixed logic when just mousing over
 *  Date: 2/12/2021 - fixed main camera not being cached
 */

using UnityEngine;
using UnityEngine.Events;

public class DragInteractable : Interactable
{
    private Vector3 m_ScreenPoint;
    private Vector3 m_Offset;
    private Rigidbody m_Rigidbody;
    private bool m_OnStopDragEvent = false;

    [Range(0.1f, 25f)]
    [SerializeField] 
    public float MoveSpeed = 5f;

    [SerializeField] 
    public UnityEvent OnDragStart = new UnityEvent();
    [SerializeField] 
    public UnityEvent OnStopDrag = new UnityEvent();

    private Camera m_mainCamera;

    protected void Awake()
    {
        InitializeFields();
        InitialiseListeners();
    }

    protected void InitializeFields()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_mainCamera = Camera.main;
    }

    protected void InitialiseListeners()
    {
        OnDragStart.AddListener(() => { m_OnStopDragEvent = false; });
        OnStopDrag.AddListener(() => { m_OnStopDragEvent = true; });
    }

    public void StorePosition()
    {
        m_ScreenPoint = m_mainCamera.WorldToScreenPoint(transform.position);

        m_Offset = transform.position - m_mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z));
    }

    private void Drag()
    {
        var curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z);

        var curPosition = m_mainCamera.ScreenToWorldPoint(curScreenPoint) + m_Offset;
        transform.position = Vector3.MoveTowards(transform.position, curPosition, MoveSpeed * Time.deltaTime);
    }

    protected override void OnMouseDown()
    {
        StorePosition();
        if (m_Rigidbody != null)
        {
            m_Rigidbody.isKinematic = true;
        }
        base.OnMouseDown();
    }

    protected override void OnMouseUp()
    {
        if (m_Rigidbody != null)
        {
            m_Rigidbody.isKinematic = false;
        }
        if (!m_OnStopDragEvent)
        {
            OnStopDrag?.Invoke();
        }
        base.OnMouseUp();
    }

    private void OnMouseOver()
    {
        // If mouse 0 has just been pressed this frame, trigger the OnDragStart event and drag
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnDragStart?.Invoke();
            Drag();
        }
        // Otherwise if mouse 0 is just being held down and OnStopDrag has not been invoked, drag
        else if (Input.GetKey(KeyCode.Mouse0) && !m_OnStopDragEvent)
        {
            Drag();
        }
    }

    private void OnMouseExit()
    {
        if (m_Rigidbody != null && Input.GetMouseButton(0))
        {
            m_Rigidbody.isKinematic = false;
        }

        if (!m_OnStopDragEvent)
            OnStopDrag?.Invoke();
    }
}