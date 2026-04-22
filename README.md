# NorbertBlaszczyk_ZadanieRekrutacyjne_AssemblyTable

Release:https://github.com/Whigle/NorbertBlaszczyk_ZadanieRekrutacyjne_AssemblyTable/releases/tag/v0.1
Wersja unity: 2022.3.62f3

Instrukcja uruchomienia:
Pobierz i wypakuj archiwum "Build_0.1v.zip" z zamieszczonego wyżej release. Uruchom aplikację używając pliku AssemblyTable.exe
lub
Sklonuj repozytorium z brancha main i uruchom projekt w edytorze Unity (preferowana wersja unity wyżej). Otwórz szcene Assets/Scenes/AssemblyScene i wciśnij play.

Instrukcja obsługi:
UI:
Po lewej lista dostępnych elementów. Wybranie opcji spowoduje stworzenie elementu przed kamerą. List posiada filtr po kategorii u góry. W górnym prawym rogu listy znajduje się przycisk do chowania/pokazywania listy
W górnej części znajdują się przyciski od sapisywani i ładowania zapisanego układu. W systemie może istnieć jednocześnie tylko jeden plik zapisu.
W dolnej częsci ekranu jest przycisk do wybrania trybu ewaluacji. Są dwa tryby: Learning który będzie informował o błędach w układzie z każdą zmianą, oraz Test, który poinformuje tylko w przypadku wciśnięcia przycisku "Validate System". Informacje te będę wyświetlane po prawej stronie.

Po wciśnięciu PPM na element na scenie pojawi się menu kontekstowe z dwiema opcjami. Opcja Edit wyświetla informacje o elemencie. Opcja Delete usuwa element ze sceny.

Scena:
Po stworzeniu elementu na scenie możemy go przesunąć po obszarze stołu przytrzymując na obiekcie LPM i przesuwając myszą. 
Na elemencie widoczne są sfery po lewej i prawej stronie. Zielone sfery po lewej to wejścia, czerwone po prawej to wyjścia. Pożna utworzyć połączenie między elementami klikając LPM na wyjście jednego elementu i klikając LPM na wejście innego elementu. Można przerwać tworzenie połączenia wciskąjąc PPM. Istniejące połączenie można również usunąć wciskając PPM na dowolny z połączonych portów. 

