using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Heap 
{
    public List<Node> m_tHeap = new List<Node>();
 
    public bool CompareFunc(Node a, Node b)
    {
        if (a.gScore < b.gScore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	public void Add(Node data)
	{
		m_tHeap.Add(data);
		UpHeap(m_tHeap.Count - 1);
	}

    public Node Pop()
	{
        Node tTemp = m_tHeap[0];
		DownHeap();

		return tTemp;
	}

	int GetParent(int nIndex)
	{
        return nIndex / 2;
	}

	int GetChild1(int nIndex)
	{
        return nIndex * 2;
	}

	int GetChild2(int nIndex)
	{
        return nIndex * 2 + 1;
	}

	void UpHeap(int nIndex)
	{
		Node tTemp = m_tHeap[nIndex];

		while (true)
		{
			if (GetParent(nIndex) < 0)
			{
				break;
			}

			if (CompareFunc(m_tHeap[nIndex], m_tHeap[GetParent(nIndex)]))
			{
				m_tHeap[nIndex] = m_tHeap[GetParent(nIndex)];
				m_tHeap[GetParent(nIndex)] = tTemp;

				nIndex = GetParent(nIndex);
			}
			else
			{
				break;
			}
		}
	}

	void DownHeap()
	{
		// copy last element to first
		m_tHeap[0] = m_tHeap[m_tHeap.Count-1];
		// delete last one
		m_tHeap.RemoveAt(m_tHeap.Count -1);

		// nothing needs to be done if there's nothing to do sthgindinasfinas
		if (m_tHeap.Count <= 0)
		{
			return;
		}
		
		Node tTemp = m_tHeap[0];
		int nCurrentIndex = 0;

		while (true)
		{
			if (GetChild1(nCurrentIndex) < m_tHeap.Count)
			{
				// has left child
				if (GetChild2(nCurrentIndex) < m_tHeap.Count)
				{
					// has right child
					if (CompareFunc(m_tHeap[GetChild1(nCurrentIndex)], m_tHeap[GetChild2(nCurrentIndex)]))
					{
						// child 1 is less than child 2
						if (CompareFunc(m_tHeap[GetChild1(nCurrentIndex)], m_tHeap[nCurrentIndex]))
						{
							// child 1 is less than parent
							m_tHeap[nCurrentIndex] = m_tHeap[GetChild1(nCurrentIndex)];
							m_tHeap[GetChild1(nCurrentIndex)] = tTemp;
							nCurrentIndex = GetChild1(nCurrentIndex);
						}
						else
						{
							break;
						}
					}
					else
					{
						// child 2 is less than child 1
						if (CompareFunc(m_tHeap[GetChild2(nCurrentIndex)], m_tHeap[nCurrentIndex]))
						{
							// child 2 is less than parent
							m_tHeap[nCurrentIndex] = m_tHeap[GetChild2(nCurrentIndex)];
							m_tHeap[GetChild2(nCurrentIndex)] = tTemp;
							nCurrentIndex = GetChild2(nCurrentIndex);
						}
						else
						{
							break;
						}
					}
				}
				else 
				{
					// doesnt have 2nd child
					if (CompareFunc(m_tHeap[GetChild1(nCurrentIndex)], m_tHeap[nCurrentIndex]))
					{
						// child is less than parent
						m_tHeap[nCurrentIndex] = m_tHeap[GetChild1(nCurrentIndex)];
						m_tHeap[GetChild1(nCurrentIndex)] = tTemp;
						nCurrentIndex = GetChild1(nCurrentIndex);
					}
					else
					{
						break;
					}
				}
			}
			else
			{
				break;
			}
		}
	}
};

