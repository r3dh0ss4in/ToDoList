using System.Text.Json;
using System;

class FileOperation {
	public void Create(string now) {
		string fileName="activity.txt";
		if(!File.Exists(fileName)) {
			File.WriteAllText(fileName,now);
		} else {
			File.AppendAllText(fileName,"\n");
			File.AppendAllText(fileName,now);
		}
	}
}

class ToDoItem {
	public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

class FinalOperation {
	private string fileName="data.json";
	public async Task AddToJson(List<ToDoItem> items) {
		var options=new JsonSerializerOptions{WriteIndented=true};
		using FileStream fs=File.Create(fileName);
		await JsonSerializer.SerializeAsync(fs, items, options);
	}
	public async Task RemoveFromjson() {
		var dataLst=new List<ToDoItem>();
		var fileName=@"data.json";
		if (File.Exists(fileName)) {
        	using (var stream = File.OpenRead(fileName))
        	using (var resp = await JsonDocument.ParseAsync(stream)) {
            	foreach (var elem in resp.RootElement.EnumerateArray()) {
                	string name = elem.GetProperty("Name").GetString() ?? string.Empty;
                	string date = elem.GetProperty("Date").GetString() ?? string.Empty;
                	dataLst.Add(new ToDoItem { Name = name, Date = date });
            	}
        	}
    	}
    	try {
    		Console.Write($"Enter Id(1-{dataLst.Count}): ");
			string input=Console.ReadLine() ?? "";
			if(string.IsNullOrEmpty(input)) {
				throw new ArgumentNullException("Please Enter a Id");
			}
			int index=Convert.ToInt32(input);
			if(index>dataLst.Count||index<0) {
				throw new ArgumentException($"Id can't be greater than {dataLst.Count} and less than 1");
			}
			index--;
			dataLst.RemoveAt(index);
		} catch(ArgumentNullException e) {
			Console.WriteLine($"Error: {e.Message}");
		} catch(ArgumentException e) {
			Console.WriteLine($"Error: {e.Message}");
		} catch(OverflowException) {
			Console.WriteLine("Number was too big or too small for Int32");
		} catch(FormatException) {
			Console.WriteLine("Invalid Input! Please enter valid Id");
		} catch(Exception e) {
			Console.WriteLine($"Error: {e.Message}");
		}
		await AddToJson(dataLst);
		await ReadFromJson();
	}
	public async Task ReadFromJson() {
		var dataLst=new List<(string name, string date)>();
		var fileName=@"data.json";
		using var stream = File.OpenRead(fileName);
		using var resp=await JsonDocument.ParseAsync(stream);
		foreach(var elem in resp.RootElement.EnumerateArray()) {
			string name = elem.GetProperty("Name").GetString() ?? string.Empty;
            string date = elem.GetProperty("Date").GetString() ?? string.Empty;
            dataLst.Add((name,date));
		}
		Console.WriteLine("-------------------------::::-------------------------");
		Console.WriteLine("-------------------------::::-------------------------");
		Console.WriteLine("-------------------------::::-------------------------");
		int id=1;
		foreach(var u in dataLst) {
			Console.Write($"{id++}: ");
			Console.WriteLine($"{u.name} : {u.date}");
		}
		Console.WriteLine("-------------------------::::-------------------------");
		Console.WriteLine("-------------------------::::-------------------------");
		Console.WriteLine("-------------------------::::-------------------------");
	}
	public async Task NewDataAdd(List<ToDoItem> newItems) {
		var dataLst=new List<ToDoItem>();
		var fileName=@"data.json";
		if (File.Exists(fileName)) {
        	using (var stream = File.OpenRead(fileName))
        	using (var resp = await JsonDocument.ParseAsync(stream)) {
            	foreach (var elem in resp.RootElement.EnumerateArray()) {
                	string name = elem.GetProperty("Name").GetString() ?? string.Empty;
                	string date = elem.GetProperty("Date").GetString() ?? string.Empty;
                	dataLst.Add(new ToDoItem { Name = name, Date = date });
            	}
        	}
    	}
		dataLst.AddRange(newItems);
		await AddToJson(dataLst);
	}
}

class ToDoList {
	public static async Task Main(string[] args) {
		Console.WriteLine("---::ToDoList---");
		DateTime now=DateTime.Now;
		Console.WriteLine(now);
		FileOperation obj=new FileOperation();
		FinalOperation fo=new FinalOperation();
		string timeNow="";
		timeNow+=now;
		obj.Create(timeNow);
		while(true) {
			Console.WriteLine("1 Get your ToDoList");
			Console.WriteLine("2 Add New Activity");
			Console.WriteLine("3 Remove Activity");
			Console.WriteLine("0 Exit");
			try {
	    		Console.Write($"Enter Number(0-3): ");
				string input=Console.ReadLine() ?? "";
				if(string.IsNullOrEmpty(input)) {
					throw new ArgumentNullException("Please Enter a Number");
				}
				int n=Convert.ToInt32(input);
				obj.Create(timeNow);
				if(n==0) {
					Console.WriteLine("---ToDoList---");
					Console.WriteLine(now);
					Console.WriteLine("Exited");
					break;
				} else if(n==1) {
					Console.WriteLine("Your ToDoList.......");
					await fo.ReadFromJson();
				} else if(n==2) {
					Console.WriteLine("How many activities would you like to add?(<=20)");
					string input2=Console.ReadLine() ?? "";
					if(string.IsNullOrEmpty(input2)) {
						throw new ArgumentNullException("Please Enter a Number");
					}
					var lst=new List<(int year, int month, int day, string name)> ();
					int x=Convert.ToInt32(input);
					if(x>20||x<=0) {
						throw new ArgumentException($"Id can't be greater than 20 and less than 1");
					}
					for(int i=0; i<x; i++) {
						Console.Write("Activity Name: ");
						string actName=Console.ReadLine() ?? "";
						if(string.IsNullOrEmpty(actName)) {
							throw new ArgumentNullException("Please Enter a Name");
						}
						Console.WriteLine("Please enter the activity deadline (DD/MM/YYYY):");
						Console.Write("DD: ");
						string input3=Console.ReadLine() ?? "";
						if(string.IsNullOrEmpty(input3)) {
							throw new ArgumentNullException("Please Enter a Number");
						}
						int actDay=Convert.ToInt32(input3);

						Console.Write("MM: ");
						string input4=Console.ReadLine() ?? "";
						if(string.IsNullOrEmpty(input4)) {
							throw new ArgumentNullException("Please Enter a Number");
						}
						int actMonth=Convert.ToInt32(input4);

						Console.Write("YYYY: ");
						string input5=Console.ReadLine() ?? "";
						if(string.IsNullOrEmpty(input5)) {
							throw new ArgumentNullException("Please Enter a Number");
						}
						int actYear=Convert.ToInt32(input5);
						lst.Add((actYear,actMonth,actDay,actName));
					}
					lst.Sort();
					var todo=new List<ToDoItem>();
					foreach(var u in lst) {
						string date="";
						date+=u.day;
						date+="-";
						date+=u.month;
						date+="-";
						date+=u.year;
						todo.Add(new ToDoItem { Name = u.name, Date = date });
					}
					await fo.NewDataAdd(todo);
				} else if(n==3) {
					await fo.ReadFromJson();
					await fo.RemoveFromjson();
				}
				if(n>3||n<0) {
					throw new ArgumentException($"Id can't be greater than 3 and less than 0");
				}
			} catch(ArgumentNullException e) {
				Console.WriteLine($"Error: {e.Message}");
			} catch(ArgumentException e) {
				Console.WriteLine($"Error: {e.Message}");
			} catch(OverflowException) {
				Console.WriteLine("Number was too big or too small for Int32");
			} catch(FormatException) {
				Console.WriteLine("Invalid Input! Please enter valid Id");
			} catch(Exception e) {
				Console.WriteLine($"Error: {e.Message}");
			}
		}
	}
}