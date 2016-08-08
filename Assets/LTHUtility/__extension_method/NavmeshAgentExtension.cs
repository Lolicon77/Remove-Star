using UnityEngine;
using System.Collections;

public static class NavmeshAgentExtension  {


		public static bool IsArrive (this NavMeshAgent agent)
		{
				if (agent.remainingDistance <= agent.stoppingDistance + 0.5f )
				{
						return true;
				}
				return false;
		}

}
