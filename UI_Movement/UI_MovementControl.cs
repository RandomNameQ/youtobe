#region

using System;
using System.Collections;
using ModestTree;
using UnityEngine;

#endregion

public class UI_MovementControl : MonoBehaviour
{
    [SerializeField] private DisappearSide       disappearSide;
    [SerializeField] private DisappearType       disappearType;
    [SerializeField] private UISettings          settings;
    [SerializeField] private CurrentVisibleState currentVisibleState;
    [SerializeField] private TranslateVariant    translateVariant;

    private RectTransform rectTransform;
    private Vector3       defaultPosition;
    public float         doubleClickThreshold = 0.15f; // Порог для определения двойного клика


    public KeyCode hotKey = KeyCode.Tab;

    // за сколько времени уи движется от А до Б точки
    public  float translateTime = 1f;
    // как далеко будет уходить обьект
    public  float distance      = 999;
    private bool  corotineIsRunning;

    private bool    isDragging;
    private float   lastClickTime;
    private Vector2 offset;

    [SerializeField]
    private AnimationCurve customCurve;
    private void Start()
    {
        rectTransform   = GetComponent<RectTransform>();
        defaultPosition = rectTransform.position;
    }

    private void Update()
    {
        if ( isDragging )
            // Если мы перетаскиваем, следим за позицией мыши
            UpdateDragging();
        if ( ( settings & UISettings.CanChangePosition ) != 0 && IsPointerOverUI() )
        {
            if ( Input.GetMouseButton(0) ) StartDragging();


            if ( Input.GetMouseButtonUp(0) )
                // Если мы отпустили кнопку мыши, заканчиваем перетаскивание
                StopDragging();

            if ( Input.GetMouseButtonDown(0) )
            {
                if ( Time.time - lastClickTime < doubleClickThreshold )
                    // Двойной клик обнаружен, возвращаем блок на стандартное место
                    ResetToDefaultPosition();

                lastClickTime = Time.time;
            }
        }

        if ( ( settings & UISettings.CanHided ) != 0 && Input.GetKeyDown(hotKey) ) HideShowBlock();

    }

    public void HideShowBlock()
    {
        if ( !corotineIsRunning )
        {
            StartCoroutine(AnimateTranslateUI());
            // если видим обьект, то переключаем на хайд, иначе на висибле
        }
    }

    public IEnumerator AnimateTranslateUI()
    {
        corotineIsRunning = true;
        if ( disappearType == DisappearType.Moving )
        {
        
            var      direction = GetDirection();
            var      startPos  = rectTransform.position;
            Vector3  endPos    = default;


            // если обьект за экраном
            if ( ( settings & UISettings.ScreenOut ) != 0 )
            {
                if ( currentVisibleState == CurrentVisibleState.IsVisible )
                    endPos = defaultPosition;
                else
                    endPos = GetScreenCenter();
            }
            else
            {

                if ( currentVisibleState == CurrentVisibleState.IsVisible )
                    endPos = rectTransform.position + direction * distance;
                else
                    endPos = defaultPosition;
            }


            var elapsedTime = 0f;

            
            if ( translateVariant == TranslateVariant.SmoothStep )
            {
                while (elapsedTime < translateTime)
                {
                    float t = Mathf.SmoothStep(0f, 1f, elapsedTime / translateTime);
                    rectTransform.position =  Vector3.Lerp(startPos, endPos, t);
                    elapsedTime            += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                while (elapsedTime < translateTime)
                {
                    float t = customCurve.Evaluate(elapsedTime / translateTime);
                    rectTransform.position =  Vector3.Lerp(startPos, endPos, t);
                    elapsedTime            += Time.deltaTime;
                    yield return null;
                }
            }
            

            // Убедитесь, что объект точно приходит в конечное положение
            rectTransform.position = endPos;
        }
        else
        {
            
            if ( ( settings & UISettings.ScreenOut ) != 0 )
            {
                if ( currentVisibleState == CurrentVisibleState.IsVisible )
                    rectTransform.position = defaultPosition;
                else
                    rectTransform.position = GetScreenCenter();
            }
            else
            {

                if ( currentVisibleState == CurrentVisibleState.IsVisible )
                    rectTransform.position = (Vector2) defaultPosition * 999;
                else
                    rectTransform.position = (Vector2) defaultPosition;
            }
            
           
        }

        currentVisibleState = currentVisibleState == CurrentVisibleState.IsVisible
            ? CurrentVisibleState.IsHided
            : CurrentVisibleState.IsVisible;
        corotineIsRunning = false;
        yield return null;
    }

    private void StartDragging()
    {
        isDragging = true;

        // Сохраняем смещение от точки касания до центра объекта
        offset = (Vector2) rectTransform.position - (Vector2) Input.mousePosition;
    }

    private void UpdateDragging()
    {
        // Обновляем позицию объекта в соответствии с мышью и смещением
        rectTransform.position = (Vector2) Input.mousePosition + offset;
    }

    private void StopDragging()
    {
        isDragging = false;
    }

    private void ResetToDefaultPosition()
    {
        rectTransform.position = (Vector2) defaultPosition;
        isDragging             = false;
    }


    private bool IsPointerOverUI()
    {
        // Проверяем, наводится ли указатель мыши на объект UI
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
    }
    

    public Vector3 GetDirection()
    {
        Vector3 direction = default;
        switch (disappearSide)
        {
            case DisappearSide.Top:
                direction = Vector3.up;
                break;
            case DisappearSide.Down:
                direction = Vector3.down;
                break;
            case DisappearSide.Right:
                direction = Vector3.right;
                break;
            case DisappearSide.Left:
                direction = Vector3.left;
                break;
            default:
                Debug.Log("баг");
                break;
        }

        return direction;
    }

    public Vector2 GetScreenCenter()
    {
        int x   = Screen.width  / 2;
        int y   = Screen.height / 2;
        var pos = new Vector2(x, y);
        return pos;
    }

   


    private enum DisappearSide
    {
        Top,
        Down,
        Right,
        Left
    }

    private enum DisappearType
    {
        Moving,
        InstantlyDisappear
    }

    [Flags]
    private enum UISettings
    {
        CanChangePosition = 1 << 0,
        CanHided          = 1 << 1,
        ScreenOut         = 1 << 2
    }


    private enum CurrentVisibleState
    {
        IsVisible,
        IsHided
    }
    
    private enum TranslateVariant
    {
        SmoothStep,
        AnimationCurve
    }

}
