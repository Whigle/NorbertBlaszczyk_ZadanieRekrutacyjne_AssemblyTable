# NorbertBlaszczyk_ZadanieRekrutacyjne_AssemblyTable

### [Release v0.1](https://github.com/Whigle/NorbertBlaszczyk_ZadanieRekrutacyjne_AssemblyTable/releases/tag/v0.1)
### Wersja unity: 2022.3.62f3

## Instrukcje
### Uruchomienia:
Pobierz i wypakuj archiwum "Build_0.1v.zip" z zamieszczonego wyżej release. Uruchom aplikację używając pliku AssemblyTable.exe
lub
Sklonuj repozytorium z brancha main i uruchom projekt w edytorze Unity (preferowana wersja unity wyżej). Otwórz szcene Assets/Scenes/AssemblyScene i wciśnij play.

### Obsługi:
UI:
Po lewej lista dostępnych elementów. Wybranie opcji spowoduje stworzenie elementu przed kamerą. List posiada filtr po kategorii u góry. W górnym prawym rogu listy znajduje się przycisk do chowania/pokazywania listy
W górnej części znajdują się przyciski od sapisywani i ładowania zapisanego układu. W systemie może istnieć jednocześnie tylko jeden plik zapisu.
W dolnej części ekranu jest przycisk do wybrania trybu ewaluacji. Są dwa tryby: Learning który będzie informował o błędach w układzie z każdą zmianą, oraz Test, który poinformuje tylko w przypadku wciśnięcia przycisku "Validate System". Informacje te będą wyświetlane po prawej stronie.

Po wciśnięciu PPM na element na scenie pojawi się menu kontekstowe z dwiema opcjami. Opcja Edit wyświetla informacje o elemencie. Opcja Delete usuwa element ze sceny.

Scena:
Po stworzeniu elementu na scenie możemy go przesunąć po obszarze stołu przytrzymując na obiekcie LPM i przesuwając myszą. 
Na elemencie widoczne są sfery po lewej i prawej stronie. Zielone sfery po lewej to wejścia, czerwone po prawej to wyjścia. Można utworzyć połączenie między elementami klikając LPM na wyjście jednego elementu i klikając LPM na wejście innego elementu. Można przerwać tworzenie połączenia wciskając PPM. Istniejące połączenie można również usunąć wciskając PPM na dowolny z połączonych portów. 

## Decyzje architektoniczne:
Wykorzystałem addressable do zredukowania zapotrzebowania pamięci w runtime, dzięki czemu do pamięci są ładowanie jedynie elementy potrzebne przy aktualnym stanie systemu (aktualnie wpływ jest pomijalny, ale przy większej liczbie elementów, z ciężkimi modelami i teksturami będzie to miało znaczenie)
Docelowo wszystkie zależności rozwiązywane w AssemblyTableBootstrapper, dzięki czemu zależności staną się jawne i łatwo weryfikowalne w jednym punkcie.

Serializacja została stworzona w uproszczonej wersji. SystemSerializer bazując na implementacjach ISystemElementsSaveDataProvider oraz IConnectionsSaveDataProvider dokonuje serializacji i deserializacji do i z odpowiednich typów (odpowiednio ElementsSaveData oraz ConnectionsSaveData). 

Projekt miał być podzielony na 3 modułu Core, App i UI. W związku z ograniczonym czasem nie rozdzieliłem wszystkiego odpowiednio. 
W moim założeniu w części Core zostały by elementy najbardziej bazowe, tak aby zawierały możliwie najmniej zależności od silnika Unity (Jeśli dałoby się to przetestować bez potrzeby odpalania sceny, lub nawet całkowicie wyciągnąć z Unity bez potrzeby dokonywania zmian). UI tłumaczy się samo. Do app z kolei trafiłoby wszystko klasy wymagające interakcji ze sceną.  

Walidacja odbywa się na bazie walidatorów dostarczanych przez providery. Providerami tymi są ScriptableObjecty dziedziczące po klasie LayoutValidatorProviderSO, co pozwala na rozszerzenie listy walidatorów bez zmiany dotychczas istniejącego kodu. Pozwala to również na łatwe zaimplementowanie bazującego na danych z LayoutTemplateSO walidatora TemplateComplianceValidator. 

Tryb oceny kontrolowany przez EvaluationModeController, część modułu Core. aby był dostępny dla komponentu reagującego na zmiany układu w trybie learning (SystemChangedValidator z modułu App), oraz wymuszonej walidacji z przycisku (ValidateBtn modułu UI)

## Lista TODO:
1. Poprawienie inputu: użycie NewInputManagera i podpięcie do eventów akcji zamiast sprawdzania wciśnięć myszy w update. Ponadto należy przy inpucie dodać weryfikowalny stan który pozwalał by na ograniczenie inputów w zależności od wykonywanej czynności (np. brak możliwości przeciągania elementów podczas tworzenia połączenia)  
2. Poprawienie zależności: Pozbycię się pozostałych Singletonów, łączenie zależności przez AssemblyTableBootstrapper
3. Uporządkowanie Core, App, UI: Rozdzielenie klas z modułu core i przeniesienie klas których funkcjonalność łączy się ze sceną do modułu app, tak jak zostało to częściowo wykonane z SystemElementSpawner i klasami powiązanymi.
4. Wersjonowanie serializacji: Dodanie saveFormatVersion i metod do konwertowania starszych wersji zapisu do nowszych po zmianie sposobu zapisu.
5. Oparcie o Interfejsy z Core: większość klas połączonych jest sztywno, należy wprowadzić poprawki analogiczne do SystemElementSpawner. Klasy z sekcji App powinny korzystać z interfejsów zadeklarowanych w sekcji core (jak ISystemElementsSaveDataProvider, ILayoutStateProvider, ISystemElementSpawner), co pozwoli na ich połączenie z elementami sekcji UI nie znających App.
