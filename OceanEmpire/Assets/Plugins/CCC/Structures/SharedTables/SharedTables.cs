using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(menuName = "CCC/Other/Shared Tables")]
public class SharedTables : ScriptableObject
{
    public class Table
    {
        public List<object> seats = new List<object>();

        private bool _removeEmptySeats;
        private int _maxSeats = -1;

        public int SeatCount { get { return seats.Count; } }
        public int ClientCount { get; private set; }
        public int MaxSeats
        {
            get { return _maxSeats; }
            set
            {
                _maxSeats = value;
                if (_maxSeats >= 0 && seats.Count > _maxSeats)
                {
                    for (int i = seats.Count - 1; i >= _maxSeats; i--)
                    {
                        _RemoveClientFromSeat(i);
                    }
                }
            }
        }
        public bool RemoveEmptySeats
        {
            get { return _removeEmptySeats; }
            set
            {
                _removeEmptySeats = value;
                if (value)
                {
                    _RemoveEmptySeats();
                }
            }
        }

        public Table(bool removeEmptySeats, int maxSeats)
        {
            _removeEmptySeats = removeEmptySeats;
            _maxSeats = maxSeats;
        }

        public bool IsAvailable()
        {
            return ClientCount < _maxSeats || _maxSeats == -1;
        }

        public int AddClient(object client)
        {
            for (int i = 0; i < seats.Count; i++)
            {
                if (seats[i] == null)
                {
                    seats[i] = client;
                    ClientCount++;
                    return i;
                }
            }
            seats.Add(client);
            ClientCount++;
            return seats.Count - 1;
        }

        public int ReleaseSeats(object client)
        {
            int removeCount = 0;
            for (int i = seats.Count - 1; i >= 0; i--)
            {
                if (seats[i] != null && seats[i].Equals(client))
                {
                    _RemoveClientFromSeat(i);
                    removeCount++;
                }
            }
            return removeCount;
        }

        public bool ReleaseSeat(object client)
        {
            for (int i = seats.Count - 1; i >= 0; i--)
            {
                if (seats[i] != null && seats[i].Equals(client))
                {
                    _RemoveClientFromSeat(i);
                    return true;
                }
            }
            return false;
        }

        private bool _RemoveClientFromSeat(int seat)
        {
            bool result;
            if (seats[seat] == null)
            {
                result = false;
            }
            else
            {
                ClientCount--;
                seats[seat] = null;
                result = true;
            }

            if (_removeEmptySeats)
                seats.RemoveAt(seat);

            return result;
        }

        private void _RemoveEmptySeats()
        {
            seats.RemoveNulls();
        }

        public void Clear()
        {
            ClientCount = 0;
            seats.Clear();
        }
    }

    [SerializeField, Header("Seat Assignment Algorithm")]
    bool spreadClients = true;
    public bool SpreadClients { get { return spreadClients; } set { spreadClients = value; } }

    [SerializeField]
    bool prioritizeTablesInOrder = false;
    public bool PrioritizeTablesInOrder { get { return prioritizeTablesInOrder; } set { prioritizeTablesInOrder = value; } }

    [SerializeField]
    bool removeEmptySeats = true;
    public bool RemoveEmptySeats
    {
        get { return removeEmptySeats; }
        set
        {
            removeEmptySeats = value;
            if (tables != null)
                for (int i = 0; i < tables.Count; i++)
                {
                    tables[i].RemoveEmptySeats = value;
                }
        }
    }

    [SerializeField, Header("-1 == infinite")]
    int maxSeatsPerTable = -1;
    public int MaxSeatsPerTable
    {
        get { return maxSeatsPerTable; }
        set
        {
            maxSeatsPerTable = value;
            if (tables != null)
                for (int i = 0; i < tables.Count; i++)
                {
                    tables[i].MaxSeats = value;
                }
        }
    }

    [SerializeField, ReadOnlyInPlayMode]
    int startingTables = 2;

    [System.NonSerialized]
    List<Table> tables;

    public bool TakeSeat(object client, out int table, out int seat)
    {
        if (client == null)
            throw new System.Exception("Null Client");

        CheckArray();

        table = -1;
        seat = -1;

        //Doit-on répartir les client de manière égal ?
        if (spreadClients)
        {
            //La quantité de client la plus basse à une table
            int minClientCount = int.MaxValue;
            for (int i = 0; i < tables.Count; i++)
            {
                if (tables[i].ClientCount < minClientCount)
                    minClientCount = tables[i].ClientCount;
            }

            //Tous nos tables sont pleine ?
            if (maxSeatsPerTable >= 0 && minClientCount >= maxSeatsPerTable)
                return false;

            //Assigne-t-on les tables des manière aléatoire ou ordonné ?
            if (!prioritizeTablesInOrder)
            {
                //On ajoute tous les tables potentielles dans une liste pour pouvoir piger aléatoirement
                List<int> potentialTables = new List<int>(tables.Count);
                for (int i = 0; i < tables.Count; i++)
                {
                    if (tables[i].ClientCount == minClientCount)
                    {
                        potentialTables.Add(i);
                    }
                }

                table = potentialTables[Random.Range(0, potentialTables.Count)];
            }
            else
            {
                //On cherche la première table avec une quantité minimal de client
                for (int i = 0; i < tables.Count; i++)
                {
                    if (tables[i].ClientCount == minClientCount)
                    {
                        table = i;
                        break;
                    }
                }
            }
        }
        else
        {
            //Assigne-t-on les tables des manière aléatoire ou ordonné ?
            if (!prioritizeTablesInOrder)
            {
                //Avons-nous une qqt infinie de siege par table ?
                if (maxSeatsPerTable == -1)
                {
                    //On choisie une table au hazard
                    table = Random.Range(0, tables.Count);
                }
                else
                {
                    //On choisie une table de maniere aleatoire, puis on verifie qu'elle a assez de siege
                    // On la retire de la liste de pige si ce n'est pas le cas

                    //On remplie un array avec la liste des tables
                    int tableCount = tables.Count;
                    int[] tableIndexes = new int[tableCount];

                    for (int i = 0; i < tableIndexes.Length; i++)
                    {
                        tableIndexes[i] = i;
                    }

                    while (tableCount > 0)
                    {
                        //On pick une table au hazard dans la liste de pige
                        int random = Random.Range(0, tableCount);
                        int pickedTable = tableIndexes[random];

                        //La table a moins de client que de seige maximum ?
                        if (tables[pickedTable].ClientCount < maxSeatsPerTable)
                        {
                            //Choisie !
                            table = pickedTable;
                            break;
                        }
                        else
                        {
                            //On la retire de la liste de pige
                            tableIndexes[random] = tableIndexes[tableCount - 1];
                            tableCount--;
                        }
                    }
                }
            }
            else
            {
                //Avons-nous inifie de siege par table ?
                if (maxSeatsPerTable == -1)
                {
                    table = 0;
                }
                else
                {
                    for (int i = 0; i < tables.Count; i++)
                    {
                        if (tables[i].ClientCount < maxSeatsPerTable)
                        {
                            table = i;
                            break;
                        }
                    }
                }
            }
        }


        if (table == -1)
            return false;

        seat = tables[table].AddClient(client);
        return true;
    }

    /// <summary>
    /// Libère le seige occupé par le client. Retourne faux si ce client n'occupait pas de siege.
    /// </summary>
    public bool ReleaseSeat(object client)
    {
        CheckArray();

        for (int i = 0; i < tables.Count; i++)
        {
            if (tables[i].ReleaseSeat(client))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Libère tous les seiges occupés par le client. Retourne la quantité de sieges libérés.
    /// </summary>
    public int ReleaseSeats(object client)
    {
        CheckArray();

        int removeCount = 0;

        for (int i = 0; i < tables.Count; i++)
        {
            removeCount += tables[i].ReleaseSeats(client);
        }
        return removeCount;
    }

    /// <summary>
    /// Libère le seige occupé par le client. Retourne faux si ce client n'occupait pas de siege.
    /// </summary>
    public bool ReleaseSeatAt(int table, object client)
    {
        CheckArray();

        if (IsAValidTable(table))
            return tables[table].ReleaseSeat(client);

        return false;
    }

    /// <summary>
    /// Libère tous les seiges occupés par le client. Retourne la quantité de sieges libérés.
    /// </summary>
    public int ReleaseSeatsAt(int table, object client)
    {
        CheckArray();

        if (IsAValidTable(table))
            return tables[table].ReleaseSeats(client);

        return 0;
    }

    /// <summary>
    /// Retourne vrai si la slot n'est pas encore pleine.
    /// </summary>
    public bool IsTableAvailable(int table)
    {
        CheckArray();

        if (IsAValidTable(table))
        {
            return tables[table].IsAvailable();
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Retourne la liste des client à une table. NE PAS MODIFER
    /// </summary>
    public Table GetClientsAtTable(int tableIndex)
    {
        CheckArray();

        if (IsAValidTable(tableIndex))
            return tables[tableIndex];
        else
            return null;
    }

    /// <summary>
    /// Retourne la quantité de slot.
    /// </summary>
    public int GetTableCount()
    {
        CheckArray();
        return tables.Count;
    }

    /// <summary>
    /// Retourne la quantité de slot initiale.
    /// </summary>
    public int GetStartingTableCount()
    {
        return startingTables;
    }

    /// <summary>
    /// Set la quantité de table. ATTENTION: Les changements peuvent persisté dans l'éditeur d'une scène à l'autre lorsqu'on reste en Playmode.
    /// Ce comportement ne sera pas répliqué en Build Standalone. Utiliez Reset() dans l'éditeur au besoin.
    /// </summary>
    public void SetTableCount(int amount)
    {
        if (tables == null)
        {
            tables = new List<Table>(amount);
        }

        while (tables.Count != amount)
        {
            if (tables.Count < amount)
            {
                _AddTable();
            }
            else
            {
                _RemoveTable();
            }
        }
    }

    /// <summary>
    /// Set la quantité de table. ATTENTION: Les changements peuvent persisté dans l'éditeur d'une scène à l'autre lorsqu'on reste en Playmode.
    /// Ce comportement ne sera pas répliqué en Build Standalone. Utiliez Reset() dans l'éditeur au besoin.
    /// </summary>
    public void AddTable()
    {
        if (tables == null)
        {
            tables = new List<Table>(4);
        }
        _AddTable();
    }

    /// <summary>
    /// Set la quantité de table. ATTENTION: Les changements peuvent persisté dans l'éditeur d'une scène à l'autre lorsqu'on reste en Playmode.
    /// Ce comportement ne sera pas répliqué en Build Standalone. Utiliez Reset() dans l'éditeur au besoin.
    /// </summary>
    public void RemoveTable()
    {
        if (tables != null)
            _RemoveTable();
    }
    /// <summary>
    /// Set la quantité de table. ATTENTION: Les changements peuvent persisté dans l'éditeur d'une scène à l'autre lorsqu'on reste en Playmode.
    /// Ce comportement ne sera pas répliqué en Build Standalone. Utiliez Reset() dans l'éditeur au besoin.
    /// </summary>
    public void RemoveTable(int index)
    {
        if (tables != null && index >= 0 && index < tables.Count)
            _RemoveTable(index);
    }

    private void _AddTable()
    {
        tables.Add(new Table(removeEmptySeats, maxSeatsPerTable));
    }
    private void _RemoveTable()
    {
        tables.RemoveLast();
    }
    private void _RemoveTable(int index)
    {
        tables.RemoveAt(index);
    }


    /// <summary>
    /// Retourne la liste des tables. NE PAS MODIFIER
    /// </summary>
    public List<Table> GetTables()
    {
        CheckArray();
        return tables;
    }

    /// <summary>
    /// Remet la quantité initiale de table et les libère tous.
    /// </summary>
    public void Reset()
    {
        SetTableCount(startingTables);
        for (int i = 0; i < tables.Count; i++)
        {
            if (tables[i] == null)
                tables[i] = new Table(removeEmptySeats, maxSeatsPerTable);
            else
                tables[i].Clear();
        }
    }

    private void OnEnable()
    {
        Reset();
    }

    private void CheckArray()
    {
        if (tables == null)
        {
            SetTableCount(startingTables);
        }
    }

    private bool IsAValidTable(int index)
    {
        return index >= 0 && index < GetTableCount();
    }
    private bool IsAValidSeat(int table, int seat)
    {
        return table >= 0 && seat >= 0 && table < GetTableCount() && seat < tables[table].SeatCount;
    }
}