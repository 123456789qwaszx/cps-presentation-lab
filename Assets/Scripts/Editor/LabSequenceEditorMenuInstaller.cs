#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

[InitializeOnLoad]
public static class LabSequenceEditorMenuInstaller
{
    static LabSequenceEditorMenuInstaller()
    {
        // 서브모듈의 훅에 "우리 도메인 메뉴"를 연결
        SequenceEditorMenuHooks.ShowCommandMenu = (allTypes, onSelected, extendMenu) =>
        {
            // 1) 메뉴 생성
            var menu = new GenericMenu();

            // 2) Lab 전용 커맨드 메뉴 구성
            //    (기존 LabCommandMenuUtility.ShowCommandSelectionMenu 를
            //     "menu를 받아서 항목만 채우는" 형태로 바꾸는 걸 권장)
            LabCommandMenuUtility.BuildCommandSelectionMenu(menu, allTypes, onSelected);

            // 3) 에디터에서 넘겨준 공통 항목(Delete 등) 주입
            extendMenu?.Invoke(menu);

            // 4) 실제 표시
            menu.ShowAsContext();
            return true; // 우리가 처리했다는 뜻
        };
    }
}
#endif