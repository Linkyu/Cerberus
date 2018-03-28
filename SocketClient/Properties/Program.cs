using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

using System.Linq;


namespace ConsoleApp1 {

    class Program {

        static void Main(string[] args)
        {
            const string DNA_DATAS_FILE = @"..\..\genome-quartzjer.txt";
            using (StreamReader reader = new StreamReader(DNA_DATAS_FILE)) {
                reader.ReadLine();

                List<string> dnaSequence = File.ReadAllLines(DNA_DATAS_FILE).ToList();
                dnaSequence.RemoveAt(0);
                int i = 0;

                TaskManager mger = new TaskManager(dnaSequence);
                mger.startTask();
                var gentypeDictionary = mger.GenotypesDictionary;
                var nitrogenBases = new List<char> {'A', 'C', 'T', 'G'};

                foreach (var nitrogenBase in nitrogenBases)
                {
                    Console.Out.WriteLine( nitrogenBase + " present " + gentypeDictionary[nitrogenBase] + " fois.");
                    
                }



            }

        }


    }



   
    
    
    public class TaskManager {

        private List<string> genomeRows;

        private Task counter { get; set; }
        
        private List<Task> currentTask;

        public Dictionary<char, int> GenotypesDictionary { get; set; }
        
        public TaskManager(List<string> rows) {
            genomeRows = rows;
            Console.WriteLine("Nombre d'element : " + genomeRows.Count);
            counter = new Task(ParseGenome);
            currentTask = new List<Task>();
    
        }

        public void AddTask(Task task) {
            currentTask.Add(task);
        }

        public void startTask()
        {
            counter.Start();
            counter.Wait();

        }



        private void SortFileByPosition()
        {
            const string separator = "\t";
            var orderedLines = genomeRows.OrderBy(s => int.Parse(s.Split(separator)[2])).ToList();
            var sequencesDictionary = new Dictionary<string, int>();
                
            for (var j = 0; j < orderedLines.Count - 1; j++)
            {
                var key = orderedLines[j].Split(separator)[3] + orderedLines[j+1].Split(separator)[3];
                if (key.Length != 4 || key.Contains('D') || key.Contains('I') || key.Contains('-')) continue;
                    
                if (sequencesDictionary.ContainsKey(key))
                {
                    sequencesDictionary[key]++;
                }
                else
                {
                    sequencesDictionary[key] = 1;
                }
            }
                
            var sortedSequencesDictionary = from entry in sequencesDictionary orderby entry.Value descending select entry;

            
        }

        //Task that count the number of dashes and unknows nitrogen base
        private void ParseGenome()  {
            Console.WriteLine("Starting a new Task");
            var dashes = 0;
            var unknowns = 0;
            const string SEPARATOR = "\t";
             GenotypesDictionary = new Dictionary<char, int>();
            var line = string.Empty;
            
            for (int i = 0; i < genomeRows.Count; i++) {
                line = genomeRows[i];
                var values = line.Split(SEPARATOR);
              
                var key = values[3].ToUpper();
                if (key.Length == 2) {
                    foreach (var c in key) {
                        if (!new List<char> {'A', 'T', 'C', 'G'}.Contains(c)) continue;    // Ignore indel varients and "--"
                            
                        if (GenotypesDictionary.ContainsKey(c)) {
                            GenotypesDictionary[c]++;
                        } else {
                            GenotypesDictionary[c] = 1;
                        }
                    }
                    if (key == "--") {
                        dashes++;
                    }
                } else {
                    
                    unknowns++;
                }
            }

            
            
        }  
        
    }
    
}


/*var task = new Task<string>(Test);

//démarrer la tache
task.Start();


task.Wait();
Program prog = new Program();
Task taskCalcul = new Task(() => {
    int somme = 0;
    for (int i = 0; i < 10; i++) {
        somme += i;
    }

    Console.WriteLine(somme);
    
    throw new Exception();

});

taskCalcul.Start();
Task test = new Task(calculer);
task.ContinueWith((t1) =>
{
    Console.WriteLine(t1.Result);
});
*/