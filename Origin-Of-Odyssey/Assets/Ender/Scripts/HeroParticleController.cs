using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Ender.Scripts
{
    public class HeroParticleController : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystem> particleSystems=new List<ParticleSystem>();

        public void PlayAura()
        {
            particleSystems[0].Play();
        }
    
        public void PlayHeroPower()
        {
            particleSystems[1].Play();
        }
    
        public Task PlayHit()
        {
            particleSystems[2].Play();
            return Task.Delay(1000);
        }
    
        public void StopAura()
        {
            particleSystems[0].Stop();
        }
    
    }
}
