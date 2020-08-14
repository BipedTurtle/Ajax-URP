using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class Test : MonoBehaviour
    {
        private async void Start()
        {
            await SayHi();

            Debug.Log("i'm done with this say hi shit");
        }


        private async Task SayHi()
        {
            Debug.Log("wait till I say hi...");

            await Task.Delay(3000);

            Debug.Log("now I say hi!");
        }
    }
}
