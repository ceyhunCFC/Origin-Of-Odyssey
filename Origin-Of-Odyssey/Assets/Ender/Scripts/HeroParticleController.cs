using System.Collections.Generic;
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
    
        public void PlayHit()
        {
            particleSystems[2].Play();
        }
    
        public void StopAura()
        {
            particleSystems[0].Stop();
        }
    
    }
}
