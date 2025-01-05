using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.UI.KillsLog
{
    public class KillsLogView : MonoBehaviour
    {
        [SerializeField] private KillLogViewRow killLogViewRowPrefab;
        [SerializeField] private KillLogModel killLogModel;
        [SerializeField, Min(1)] private int kilLogMaxLenght = 1;
        [SerializeField, Min(1)] private float rowExistTime = 1;
        
        private readonly Stack<KillLogViewRow> _unActiveRows = new();
        private readonly Queue<KillLogViewRow> _activeRows = new();
        
        private void Awake()
        {
            killLogModel.OnKillLog += ShowLog;

            var rows = GetComponentsInChildren<KillLogViewRow>(true).ToList();
            for (var i = 0; i < kilLogMaxLenght - rows.Count; i++)
                rows.Add(Instantiate(killLogViewRowPrefab, transform));

            for (var i = 0; i < rows.Count; i++)
            {
                if (i >= kilLogMaxLenght)
                {
                    Destroy(rows[i].gameObject);
                }
                else
                {
                    rows[i].Initialize(rowExistTime);
                    rows[i].HideInstantly();
                    rows[i].OnExistEnd += HideRow;
                    _unActiveRows.Push(rows[i]);
                }
            }
        }

        private void ShowLog(string killer, string killed)
        {
            var row = _unActiveRows.Count > 0 
                ? _unActiveRows.Pop() 
                : _activeRows.Dequeue();
            
            row.Show($"{killer} kill-> {killed}");
            row.transform.SetAsFirstSibling();

            _activeRows.Enqueue(row);
        }

        private void HideRow()
        {
            var row = _activeRows.Dequeue();
            
            row.HideInstantly();
            
            _unActiveRows.Push(row);
        }
    }
}