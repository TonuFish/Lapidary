set compile_env: 0
! ------------------- Class definition for LapidaryRoot
expectvalue /Class
doit
Object subclass: 'LapidaryRoot'
  instVarNames: #( localNumber localString cacheAtCreation)
  classVars: #( RootCache)
  classInstVars: #()
  poolDictionaries: #()
  inDictionary: UserGlobals
  options: #()

%
expectvalue /Class
doit
LapidaryRoot category: 'Lapidary Testing'
%
! ------------------- Remove existing behavior from LapidaryRoot
removeallmethods LapidaryRoot
removeallclassmethods LapidaryRoot
! ------------------- Class methods for LapidaryRoot
category: 'Caching'
classmethod: LapidaryRoot
clearCache
		"Clears the cache."

	RootCache removeAllKeys: (RootCache keys).
%
category: 'Caching'
classmethod: LapidaryRoot
initialise
		"Create the cache."

	RootCache == nil ifTrue:[Dictionary new].
%
category: 'Caching'
classmethod: LapidaryRoot
leakCache
		"Leak the cache."

	^RootCache.
%
category: 'Object Creation'
classmethod: LapidaryRoot
newRoot
		"Creates a new root object."

	^self new.
%
category: 'Basic'
classmethod: LapidaryRoot
theNumberFourtyTwo
		"Returns SmallInteger."

	^ 42.
%
category: 'Caching'
classmethod: LapidaryRoot
tryAddCache: aKey value: aValue
		"Add KVP to cache if key is not nil and it doesn't already exist."

	aKey == nil ifTrue:[^false].
	(RootCache associationAt: aKey ifAbsent: [nil]) == nil ifFalse:[^false].
	RootCache at: aKey put: aValue.
	^true.
%
! ------------------- Instance methods for LapidaryRoot
category: 'Accessing'
method: LapidaryRoot
cacheAtCreation
		"Gets cacheAtCreation."

	^self cacheAtCreation.
%
category: 'Accessing'
method: LapidaryRoot
localNumber
		"Gets localNumber."

	^self localNumber.
%
category: 'Updating'
method: LapidaryRoot
localNumber: aNumber
		"Sets localNumber."

	localNumber := aNumber.
%
category: 'Accessing'
method: LapidaryRoot
localString
		"Gets localString."

	^self localString.
%
category: 'Updating'
method: LapidaryRoot
localString: aString
		"Sets localString."

	localString := aString.
%
