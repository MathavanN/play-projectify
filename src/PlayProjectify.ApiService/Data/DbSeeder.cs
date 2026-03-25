using Microsoft.EntityFrameworkCore;
using PlayProjectify.ApiService.Models.Entites;

namespace PlayProjectify.ApiService.Data;

public record Location(string City, string State, string PostalCode, string Country);
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Orders.AnyAsync())
            return; // Already seeded

        var random = new Random();

        var firstNames = new[]
{
    "John","Jane","Alice","Bob","Charlie","Emma","Liam","Olivia","Noah","Ava",
    "Elijah","Sophia","Lucas","Mia","Mason","Isabella","Ethan","Charlotte","Logan","Amelia",
    "James","Harper","Benjamin","Evelyn","Jacob","Abigail","Michael","Emily","Daniel","Elizabeth",
    "Henry","Sofia","Jackson","Avery","Sebastian","Ella","Aiden","Scarlett","Matthew","Grace",
    "Samuel","Chloe","David","Victoria","Joseph","Riley","Carter","Aria","Owen","Lily", "Cleveland","Lee","Lewis",
            "Eino","Marjorie","Layne","Walker","Dolores","Norwood","Tanya","Birdie","Angelo","Alejandro","Trisha",
            "Gracie","Valentina","Ernesto","Billie","Angie","Elfrieda","Eddie","Will","Clifford","Anne","Rosemary","Dion",
"Sandra", "Merlin", "Jennie", "Rocio", "Enoch", "Stewart", "Jerome", "Jacob", "Chester", "Mack", "Emilio", "Nick", 
            "Erling", "Saul", "Dan", "Felicia", "Francisco", "Ashley", "Craig", "Penny", "Kristen", "Violet", "Jeanne", "Irene", 
            "Pat", "Amanda", "Abel", "Jon", "Isaac", "Bruce", "Kurtis", "Kathleen", "Elsie", "Al", "Devin", "Lewis", "Eunice", 
            "May", "Angel", "Oda", "Ashley", "Hester", "Reed", "Monica", "Joel", "Juana", "Frankie", "Irma", "Norma", "Solon", 
            "Sally", "Nicolas", "Keely", "Jasmine", "Minnie", "Pete", "Ted", "Carole", "Dustin", "Arjun", "Dakota", "Nedra", 
            "Ferne", "Carolyn", "Shanie", "Willie", "Alex", "Hilda", "Mamie", "Jesus", "Estrella", "Gregory", "Angus", "Justine", 
            "Sarah", "Bette", "Theodore", "Priscilla", "Christy", "Bobbie", "Thomas", "Genoveva", "Herbert", "Melba", "Merl", 
            "Kristen", "Harvey", "Willard", "Marie", "Kate", "Isaias", "Rolando", "Carl", "Jarred", "Clement", "Dave", "Reece",
            "Kiara", "Stephen", "Verlie", "Christopher", "Emilie", "Nathan", "Marguerite", "Dedrick", "Mary", "Flavio", "Fanny",
            "Berry", "Odessa", "Drew", "Alba", "Gladys", "Pearl", "Kim", "Eleanora", "Dwayne", "Tamara", "Israel", "Albina", "Alton",
            "Silas", "Elaine", "Fiona", "Don", "Alisha", "Christop", "Alexzander", "Kevin", "Nicole", "Jody", "Clint", "Ryan", "Opal",
            "Marlene", "Lincoln", "Sylvester", "Ann", "Carmella", "Miracle", "Erick", "Garrett", "Theresa", "Randy", "Darla", "Braden", "Jaiden", "Roland", "Merl", "Candice", "Seth", "Gertrude", "Paulette", "Clara", "Marcus", "Stewart", "Casey", "Jake", "Jaime", "Ernestine", "Frances", "Derek", "Jayde", "Conor", "Ryan", "Edwin", "Kacey", "Keith", "Ruben", "Harriet", "Arnold", "Santos", "Christopher", "Billy", "Joey", "Lori", "Ed", "Lillian", "Emmitt", "Ricardo", "Melvin", "Manuela", "Michelle", "Claire", "Carolyn", "Violet", "Brody", "Reilly", "Taylor", "Moses", "Ronald", "Omar", "Camren", "Lucille", "Sonya", "Reagan", "Giovanna", "Rubye", "Willie", "Dora", "Kelley", "Danielle", "Norman", "Darrel", "Fannie", "Brandy", "Tyrique", "Elyse", "Alejandrin", "Devin", "Mattie", "Desiree", "Aisha", "Grady", "Grace", "Lynette", "Willie", 
            "Francisco", "Grady", "Kendra", "Melvin", "Thomas", "Elenora", "Theodora", "Nick", "Gertrude", "Cristina", "Wilma", "Yessenia", "Stanford", "Gayle", "Ruben", "Eunice", "Amos", "Ralph", "Ross", "Lucas", "Rachel", "Valerie", "Wilma", "Jay", "Renee", "Julie", "Laurie", "Belinda", "Sedrick", "Paula", "Courtney", "Peggy", "Domingo", "Erling", "Allen", "Tommy", "Doris", "Bennie", "Al", 
            "Lindsey", "Marcella", "Sergio", "Emilio", "Eva", "Stephanie", "Forrest", "Randolph", "Hassie", "Fabiola", "Sherwood", "Zack", 
            "Zoe", "Lindsay", "Shaylee", "Jeremiah", "Francis", "Roselyn", "Emilio", "Aliza", "Grover", "Casey", "Fernando", "Flora", "Meda", "Garry", "Malachi", "Owen", "Gilbert", "Dakota", "Kylee", "Austin", "Israel", "Eleanor", "Toney", "Ada", "Wilfred", "Ramon", "Stacey", "Sergio", "Anita", "Haven", "Misael", "Dean", "Tabitha", "Nikki", "Leroy", "Ramona", "Alan", "Raquel", "Caroline", "Gladys", "Melanie", "Patsy", "Justina", "Tyson", "Rose", "Oscar", "Hannah", "Phyllis", "Clint", "Erick", "Candelario", "Manuel", "Cassandre", "Jennifer", "Ignacio", "Jaime", "Spencer", "Paul", "Karli", "Florencio", "Bridget", "Eric", "Forrest", "Cedric", "Van", "Tracy", "Jayme", "Felicity", "Presley", "Jan", "Kirk", "Eunice", "Carla", "Darrin", "Ricardo", "Javier", "Santa", "Hope", "Jane", "Samantha", "Brent", "Sonia", "Haylee", "Anna", "Emmie", 
            "Valerie", "Lois", "Rene", "Lukas", "Ruby", "Bartholome", "Joshuah", "Bridgette", "Anita", "Joanny", "Laverne", "Henrietta", "Samantha", "Annie", "Terry", "Modesta", "Beverly", "Kelley", "Skylar", "Terrence", "Ervin", "Rickey", "Chester", "Camryn", "Caden", "Jesse", "Garrett", "Albert", "Katharina", "Verla", "Carole", "Ollie", "Josephine", "Leroy", "Angelita", "Sheri", "Waylon", 
            "Maria", "Verna", "Celine", "Letitia", "Kay", "Nadine", "Mohammad", "Sandra", "Tommie", "Timmy", "Lenny", "Carla", "Arthur", "Mazie", "Savanna", "Claude", "Armando", "Darrin", "Luke", "Thelma", "Dean", "Joe", "Anthony", "Heather", "Victor", "Leland", "Karl", "Martha", "Dallas", "Marjorie", "Don", "Sheryl", "Hilario", "Roberta", "Melissa", "Arthur", "Darrell", "Francisco", "Lavina", "Sharon", "Doyle", "Doris", "Hugh", "Manuel", "Vella", "Tabitha", "Lance", "Susan", "Althea", "Porter", "Stephanie", "Joseph", "Alyson", "Jerry", "Enrique", "Bruce", "Delbert", "Shannon", "Guy", "Antwon", "Frank", "Michelle", "Guadalupe", "Darrin", "Noel", "Riley", "Candace", "Frances", "Melba", "Marianne", "Georgiana", "Robbie", "Leslie", "Marie", "Idell", "Eloise", "Byron", "Jackie", "Elena", "Michale", "Ryan", "Marty", "Trenton", "Yoshiko"

};
        var lastNames = new[]
{
    "Smith","Johnson","Williams","Brown","Jones","Garcia","Miller","Davis","Rodriguez","Martinez",
    "Hernandez","Lopez","Gonzalez","Wilson","Anderson","Thomas","Taylor","Moore","Jackson","Martin",
    "Lee","Perez","Thompson","White","Harris","Sanchez","Clark","Ramirez","Lewis","Robinson",
    "Walker","Young","Allen","King","Wright","Scott","Torres","Nguyen","Hill","Flores",
    "Green","Adams","Nelson","Baker","Hall","Rivera","Campbell","Mitchell","Carter","Roberts", "Ziemann", "Gorczany-Christiansen", "Kovacek", "Schaefer", "Considine", "Breitenberg", "Dietrich", "Larson", "Rosenbaum", "Rogahn", "Heaney", "Casper", "Zieme", "Murray", "Olson", "Renner-O'Reilly", "Thompson", "Jakubowski", "Willms", "Macejkovic", "Walker", "Turcotte", "Fadel", "Thiel", "O'Connell", "Wyman", "Langworth", "Watsica", "Runolfsson", "Christiansen", "Stehr", "Wolff", "Kutch", "Stiedemann", "Schaden", "Weimann", "Bednar", "Dibbert", "Hilpert", "Gislason", "Labadie", "Lang", "Schowalter", "Kuvalis", "Ebert", "Romaguera", "Streich", "Rippin", "Hoeger", "Moen", "Bradtke", "Rau", "Stokes", "Bartell", "Ledner", "Flatley", "Cronin", "Witting", "Hagenes", "Corwin", "Nader-Doyle", "Borer", "Lemke", "Weber", "Howe", "Medhurst", "Kihn", "Trantow", "Lehner", "Marquardt-Goodwin", "Macejkovic", "Schimmel", "Von-Cartwright", "Torphy", "Lakin", "Wisozk", "Ortiz", "Paucek", "Torp", "Russel", "Baumbach", "Harris", "Turcotte", "Johns", "Greenfelder", "Purdy-Stiedemann", "Schmeler", "Kub", "Roob", "Effertz", "Effertz", "Walker", "Runolfsdottir", "Beahan", "Heathcote", "Mitchell", "Schmidt", "Hickle", "Macejkovic", "Wunsch", "Howell", "Zieme", "Kuphal", "Bode", "Hoppe", "Lehner", "Streich", "Ziemann", "Kuhn", "Beer", "Von", "Bahringer", "Smith", "Schamberger", "Marquardt", "Bayer", "Steuber", "Blanda", "Towne", "Tillman", "McCullough", "Hessel", "Vandervort", "Stoltenberg", "Bosco", "Olson", "Larkin", "Runolfsdottir-Steuber", "Walter", "Wunsch", "Jaskolski", "McGlynn", "Christiansen", "Hackett", "Jacobson-Douglas", "Smitham", "Schimmel", "Sporer",
"Murphy", "Mueller", "Hermann", "Erdman", "Kling", "Littel", "Daugherty", "Pacocha", "Crooks", "MacGyver", "Marquardt", "Grimes", "Ritchie", "Dietrich-Krajcik", "Schaefer", "VonRueden", "Beatty", "Turner", "Cassin", "Cruickshank", "Dietrich", "Rempel", "Schroeder", "Stoltenberg", "O'Reilly", "Orn", "Volkman", "Kovacek", "Grady", "Rempel", "O'Keefe", "Denesik", "Stiedemann", "Schultz", "Erdman", "Kozey", "D'Amore", "Pacocha", "Sauer", "Franey", "Blanda", "Macejkovic", "Abernathy", "Borer", "Greenfelder", "Reinger", "Kunze", "Walker", "Howell", "VonRueden", "Gibson", "Tromp", "Murphy", "Metz", "Herman", "Zemlak",
"Denesik", "Leffler", "Homenick", "Hahn", "McDermott", "Metz", "Walker", "Heathcote", "D'Amore", "Fritsch", "Leffler", "Schoen", "Halvorson", "Carroll", "Keebler", "Berge", "Crooks", "Zemlak", "McClure", "Buckridge", "Abernathy",
"Harvey", "Marquardt", "Russel", "Muller", "Altenwerth", "Beier", "Wintheiser", "Bradtke", "Nikolaus", "Tremblay", "Thiel", "Hilll", "Kautzer", "Rath", "Will", "Pacocha", "Graham", "Hegmann", "Weber", "Wisozk", "Sawayn", "Hermann", "McDermott", "Stamm", "Brown", "Sporer", "Waelchi", "Heidenreich", "Orn", "Rosenbaum", "Bernier", "Muller", "Thiel", "Harvey", "Emard", "MacGyver", "Von", "Hintz", "Pagac", "King", "Gleichner", "Hodkiewicz", "Schuppe", "Wuckert", "Wunsch", "Heller", "McLaughlin", "Zemlak", "Swaniawski", "Johns", "Dooley", "Orn", "Ullrich", "Moen", "Beahan", "Veum", "Weissnat", "Greenfelder", "Stamm",
"Grady-Hahn", "Osinski", "Kuhlman", "Stoltenberg", "Gleichner", "Bergstrom", "Howe", "Collins", "Howell", "Kuvalis", "Wiza", "Wiza", "Jakubowski-Rogahn", "Renner", "Sanford", "Larkin", "Stroman", "Mitchell-Watsica", "Nienow", "Schmitt", "Lindgren", "McLaughlin", "Baumbach", "Wiza", "Dooley", "Schumm",
"Waelchi", "Prosacco", "Dickens", "Kunde", "Abernathy", "Quitzon", "Prosacco", "Hills", "Klein",
"McCullough", "Blick-Nolan", "Kassulke", "Romaguera", "Schuster", "Aufderhar", "Kozey", "Gleichner", "Prohaska", "Thiel", "Schiller", "Kohler", "Wisozk", "Stroman", "Deckow", "Schuppe", "Swaniawski", "Harber", "Hills", "Stamm", "Bernier", "Stanton", "Parker", "Runolfsdottir", "Langworth", "Altenwerth", "Kunde", "Sauer", "Frami-Considine", "MacGyver", "Abernathy", "Jacobi", "Ankunding", "Russel", "Lakin", "Bernhard", "Marquardt", "Marvin", "Bergnaum", "Wilkinson", "Muller", "Casper", "DuBuque", "Rowe", "Wisoky", "Flatley", "Schroeder", "Wehner-Kuhic", "Kemmer", "Mills", "Bradtke", "Walter", "Johnston", "Bergnaum",
"Kling", "Graham", "Watsica", "Weimann", "Haley", "Hoeger", "Swift", "McKenzie", "McCullough-Sauer", "Marvin", "Sauer", "Lubowitz", "Stroman-Emard", "Ankunding", "Kozey", "Fahey", "Zemlak-Kunde", "Marvin", "Hauck", "Dickinson", "Schmidt", "Hammes", "Grant", "Cummerata", "Dickens", "Heaney", "Hamill-Becker", "Mraz", "Macejkovic", "Sipes-Littel", "Bayer", "Gibson", "Beer", "Moore-Jacobson", "Dibbert", "Kihn", "Durgan", "Schneider", "Koch", "Quigley-Herzog", "Stanton", "Kiehn", "Abernathy", "Hartmann", "Krajcik", "Block", "Effertz", "Nitzsche", "Greenholt", "Kuphal", "Reichel", "Morissette", "Kuhic", "Corkery", "Abernathy", "Hegmann", "Hansen", "Cruickshank-Zboncak", "Treutel", "Botsford", "Crooks", "Lowe", "Armstrong", "Ondricka", "Aufderhar", "Purdy", "Feeney", "Ryan", "Cummerata", "Kutch", "Pfannerstill", "Wuckert", "Greenfelder", "Langosh-Weimann", "Osinski", "Reynolds", "Pagac", "Stehr", "Jakubowski", "Pfeffer", "D'Amore", "Schultz", "Kuhn", "Wuckert", "Reynolds", "Farrell", "Hills", "Ledner", "Ryan", "Koepp", "Nienow", "Schroeder", "Schimmel", "Kuhic", "Walsh", "Stoltenberg", "Smitham", "O'Keefe", "Konopelski", "Jenkins", "Gerlach", "Bergstrom", "Veum", "Nitzsche", "Altenwerth", "Friesen", "Moore", "Lueilwitz", "Wintheiser-Yost", "Fahey", "Franecki", "Hartmann", "Kozey", "Wisoky", "Mertz", "Nienow", "Wisoky", "Stoltenberg", "Homenick", "Oberbrunner", "Tillman", "Jacobson", "Gusikowski", "Lynch", "Harber-Wuckert", "Robel", "Yost", "Kuphal", "Schmitt", "Larson", "DuBuque", "Schroeder", "Veum", "Fay", "Sawayn", "Pfannerstill", "Zboncak"
};
        var locations = new[]
{
    // 🇨🇭 Switzerland
    new Location("Zurich", "ZH", "8001", "Switzerland"),
    new Location("Geneva", "GE", "1201", "Switzerland"),
    new Location("Basel", "BS", "4001", "Switzerland"),
    new Location("Bern", "BE", "3001", "Switzerland"),
    new Location("Lausanne", "VD", "1000", "Switzerland"),

    // 🇩🇪 Germany
    new Location("Berlin", "BE", "10115", "Germany"),
    new Location("Munich", "BY", "80331", "Germany"),
    new Location("Hamburg", "HH", "20095", "Germany"),
    new Location("Frankfurt", "HE", "60311", "Germany"),
    new Location("Cologne", "NW", "50667", "Germany"),

    // 🇫🇷 France
    new Location("Paris", "IDF", "75001", "France"),
    new Location("Lyon", "ARA", "69001", "France"),
    new Location("Marseille", "PAC", "13001", "France"),
    new Location("Nice", "PAC", "06000", "France"),

    // 🇦🇹 Austria
    new Location("Vienna", "W", "1010", "Austria"),
    new Location("Salzburg", "S", "5020", "Austria"),
    new Location("Graz", "ST", "8010", "Austria"),

    // 🇮🇹 Italy
    new Location("Rome", "RM", "00100", "Italy"),
    new Location("Milan", "MI", "20121", "Italy"),
    new Location("Naples", "NA", "80100", "Italy"),

    // 🇪🇸 Spain
    new Location("Madrid", "MD", "28001", "Spain"),
    new Location("Barcelona", "CT", "08001", "Spain"),
    new Location("Valencia", "VC", "46001", "Spain"),

    // 🇳🇱 Netherlands
    new Location("Amsterdam", "NH", "1012", "Netherlands"),
    new Location("Rotterdam", "ZH", "3011", "Netherlands"),

    // 🇧🇪 Belgium
    new Location("Brussels", "BRU", "1000", "Belgium"),
    new Location("Antwerp", "VAN", "2000", "Belgium"),

    // 🇵🇱 Poland
    new Location("Warsaw", "MZ", "00-001", "Poland"),
    new Location("Krakow", "MA", "30-001", "Poland"),

    // 🇨🇿 Czech Republic
    new Location("Prague", "PR", "11000", "Czech Republic"),

    // 🇭🇺 Hungary
    new Location("Budapest", "BU", "1011", "Hungary"),
    // 🇺🇸 USA
    new Location("New York", "NY", "10001", "USA"),
    new Location("Los Angeles", "CA", "90001", "USA"),
    new Location("Chicago", "IL", "60601", "USA"),
    new Location("Houston", "TX", "77001", "USA"),
    new Location("Phoenix", "AZ", "85001", "USA"),
    new Location("Philadelphia", "PA", "19101", "USA"),
    new Location("San Antonio", "TX", "78201", "USA"),
    new Location("San Diego", "CA", "92101", "USA"),
    new Location("Dallas", "TX", "75201", "USA"),
    new Location("San Jose", "CA", "95101", "USA"),
    new Location("Austin", "TX", "73301", "USA"),
    new Location("Jacksonville", "FL", "32099", "USA"),
    new Location("Fort Worth", "TX", "76101", "USA"),
    new Location("Columbus", "OH", "43085", "USA"),
    new Location("Charlotte", "NC", "28201", "USA"),

    // 🇨🇦 Canada
    new Location("Toronto", "ON", "M5H", "Canada"),
    new Location("Vancouver", "BC", "V5K", "Canada"),
    new Location("Montreal", "QC", "H1A", "Canada"),
    new Location("Calgary", "AB", "T2A", "Canada"),
    new Location("Ottawa", "ON", "K1A", "Canada"),
    new Location("Edmonton", "AB", "T5A", "Canada"),
    new Location("Winnipeg", "MB", "R2C", "Canada"),
    new Location("Quebec City", "QC", "G1A", "Canada"),

    // 🇮🇳 India
    new Location("Mumbai", "MH", "400001", "India"),
    new Location("Delhi", "DL", "110001", "India"),
    new Location("Bangalore", "KA", "560001", "India"),
    new Location("Hyderabad", "TS", "500001", "India"),
    new Location("Ahmedabad", "GJ", "380001", "India"),
    new Location("Chennai", "TN", "600001", "India"),
    new Location("Kolkata", "WB", "700001", "India"),
    new Location("Pune", "MH", "411001", "India"),
    new Location("Jaipur", "RJ", "302001", "India"),
    new Location("Lucknow", "UP", "226001", "India"),

    // 🇸🇬 Singapore
    new Location("Singapore", "SG", "018989", "Singapore"),
    new Location("Singapore", "SG", "049213", "Singapore"),
    new Location("Singapore", "SG", "238801", "Singapore"),
    new Location("Singapore", "SG", "569933", "Singapore"),
    new Location("Singapore", "SG", "408600", "Singapore"),
};
        var products = new[]
{
    //Electronics
    "Laptop","Gaming Laptop","Ultrabook","Smartphone","Tablet","Smartwatch",
    "Desktop PC","All-in-One PC","Monitor","4K Monitor","Curved Monitor",
    "Keyboard","Mechanical Keyboard","Mouse","Wireless Mouse","Gaming Mouse",
    "Webcam","Microphone","Headphones","Noise Cancelling Headphones",
    "Earbuds","Bluetooth Speaker","Soundbar","Home Theater System",
    "VR Headset","Streaming Stick","Projector","Printer","Scanner",
    "External HDD","SSD Drive","USB Flash Drive","Docking Station",
    "Router","WiFi Extender","Modem","Power Bank","Charging Cable","Wireless Charger",

    //Gaming
    "Gaming Console","Game Controller","Racing Wheel","Gaming Chair",
    "Gaming Desk","VR Controllers","Capture Card",

    //Home Appliances
    "Refrigerator","Mini Fridge","Washing Machine","Dryer","Dishwasher",
    "Microwave","Oven","Toaster","Blender","Food Processor",
    "Coffee Maker","Espresso Machine","Electric Kettle","Air Fryer",
    "Rice Cooker","Slow Cooker","Vacuum Cleaner","Robot Vacuum",
    "Air Purifier","Humidifier","Dehumidifier","Fan","Heater","Smart Thermostat",

    //Furniture
    "Office Chair","Ergonomic Chair","Standing Desk","Computer Desk",
    "Dining Table","Coffee Table","Bookshelf","Wardrobe","TV Stand","Sofa",

    //Lifestyle
    "Backpack","Travel Backpack","Suitcase","Carry-On Luggage",
    "Wallet","Handbag","Sunglasses","Watch","Umbrella","Water Bottle",

    //Fitness
    "Treadmill","Exercise Bike","Elliptical Machine","Dumbbells",
    "Barbell","Kettlebell","Resistance Bands","Yoga Mat","Foam Roller",
    "Pull-Up Bar","Fitness Tracker","Gym Gloves",

    //Clothing
    "T-Shirt","Polo Shirt","Dress Shirt","Hoodie","Jacket","Jeans",
    "Shorts","Sweatpants","Dress","Skirt","Sneakers","Running Shoes",
    "Boots","Sandals","Socks","Hat","Cap",

    //Kids & Toys
    "Toy Car","Doll","Building Blocks","Puzzle","Board Game",
    "Action Figure","Remote Control Car","Stuffed Animal",

    //Pets
    "Dog Food","Cat Food","Pet Bed","Leash","Cat Litter","Aquarium",

    //Kitchen & Dining
    "Knife Set","Cutting Board","Cookware Set","Frying Pan","Saucepan",
    "Plate Set","Glassware","Mug","Wine Glass","Lunch Box",

    //Office & School
    "Notebook","Planner","Pen Set","Pencil Case","Marker Set",
    "Stapler","Desk Organizer","Whiteboard","Printer Paper",

    //Automotive
    "Car Vacuum","Dash Cam","Car Charger","Jump Starter",
    "Tire Inflator","Car Cover","Phone Mount",

    //Outdoor
    "Tent","Sleeping Bag","Camping Stove","Flashlight",
    "Hiking Backpack","Grill","Garden Hose","Lawn Mower",

    //Misc / Smart Home
    "Smart Light Bulb","Smart Plug","Security Camera","Doorbell Camera",
    "Smoke Detector","Carbon Monoxide Detector","Extension Cord"
};
        var statuses = Enum.GetValues<OrderStatus>();

        var orders = new List<Order>();
        var custmers = new List<Customer>();
        var addresses = new List<Address>();
        var items = new List<OrderItem>();

        const int totalRecords = 1000;
        const int batchSize = 1000;

        for (int i = 0; i < totalRecords; i++)
        {
            var loc = locations[random.Next(locations.Length)];

            var shippingAddress = new Address
            {
                Id = Guid.NewGuid(),
                Street1 = $"Main St {random.Next(1, 200)}",
                City = loc.City,
                State = loc.State,
                PostalCode = loc.PostalCode + random.Next(0, 99).ToString("D2"),
                Country = loc.Country
            };

            Address? billingAddress = null;

            if (random.NextDouble() > 0.3)
            {
                var loc2 = locations[random.Next(locations.Length)];

                billingAddress = new Address
                {
                    Id = Guid.NewGuid(),
                    Street1 = $"Second St {random.Next(1, 200)}",
                    City = loc2.City,
                    State = loc2.State,
                    PostalCode = loc2.PostalCode + random.Next(0, 99).ToString("D2"),
                    Country = loc2.Country
                };

                addresses.Add(billingAddress);
            }

            addresses.Add(shippingAddress);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                FirstName = firstNames[random.Next(firstNames.Length)],
                LastName = lastNames[random.Next(lastNames.Length)],
                Email = $"user{i}@example.com",
                PhoneNumber = $"+41{random.Next(100000000, 999999999)}",
                ShippingAddressId = shippingAddress.Id,
                BillingAddressId = billingAddress?.Id,
                OrderDate = DateTime.UtcNow.AddDays(-random.Next(0, 365)),
                OrderStatus = statuses[random.Next(statuses.Length)],
                Items = []
            };
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = order.FirstName,
                LastName = order.LastName,
                Email = order.Email,
                PhoneNumber = random.Next(0, 2) == 0 ? null : $"+41{random.Next(100000000, 999999999)}",
                CompanyName = random.Next(0, 3) == 0 ? $"Company-{products[random.Next(products.Length)]}" : null,
                Notes = null,
                PreferredCurrency = "CHF",
                PreferredLanguage = "en",
                MarketingOptIn = random.Next(0, 2) == 0,
                IsActive = true
            };

            int itemCount = random.Next(1, 5);
            decimal total = 0;

            for (int j = 0; j < itemCount; j++)
            {
                var quantity = random.Next(1, 4);
                var price = Math.Round((decimal)(random.NextDouble() * 500 + 20), 2);

                var item = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductName = products[random.Next(products.Length)],
                    Quantity = quantity,
                    UnitPrice = price
                };

                total += item.Quantity * item.UnitPrice;

                items.Add(item);
                order.Items.Add(item);
            }

            order.TotalAmount = total;

            orders.Add(order);
            custmers.Add(customer);

            if (i % batchSize == 0 && i > 0)
            {
                await context.AddRangeAsync(addresses);
                await context.AddRangeAsync(orders);
                await context.AddRangeAsync(items);
                await context.AddRangeAsync(custmers);

                await context.SaveChangesAsync();

                context.ChangeTracker.Clear();

                addresses.Clear();
                orders.Clear();
                items.Clear();
                custmers.Clear();
            }
        }

        // Final flush
        if (orders.Count > 0)
        {
            await context.AddRangeAsync(addresses);
            await context.AddRangeAsync(orders);
            await context.AddRangeAsync(items);
            await context.AddRangeAsync(custmers);

            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
        }
    }
}