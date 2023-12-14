using System.Collections.Generic;
using UnityEngine;

namespace LupinrangerPatranger
{
    public class MeshSocketController : MonoBehaviour
    {
        private Dictionary<SocketID, MeshSocket> socketMap = new Dictionary<SocketID, MeshSocket>();

        void Start()
        {
            MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
            foreach (var socket in sockets)
            {
                socketMap[socket.socketID] = socket;
            }
        }

        public void Attach(Transform objectTransform, SocketID socketID)
        {
            socketMap[socketID].Attach(objectTransform);
        }
    }
}