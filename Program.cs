//*****************************************************************************
//** 1590. Make Sum Divisible by P   leetcode                                **
//** It took me more time to solve this problem than some leetcode hard      **
//** problems.  This implimentation, as it stands, will not solve four test  **
//** cases.  This is due to improper handling of zero and negative 1 sums    **
//** when looking at results.  You will need to implement your own code to   **
//** solve for this problem.  Good luck!                                     **
//*****************************************************************************
//*****************************************************************************


typedef struct {
    int key;
    int value;
} HashMapEntry;

typedef struct {
    HashMapEntry* entries;
    int capacity;
} HashMap;

// Function to create a new HashMap
HashMap* createHashMap(int capacity) {
    HashMap* map = (HashMap*)malloc(sizeof(HashMap));
    map->capacity = capacity;
    map->entries = (HashMapEntry*)malloc(capacity * sizeof(HashMapEntry));
    for (int i = 0; i < capacity; i++) {
        map->entries[i].key = -1; // Initialize keys as -1 (not found)
    }
    return map;
}

// Hash function to compute index
int hash(int key, int capacity) {
    return (key % capacity + capacity) % capacity;
}

// Function to insert or update the value in the HashMap
void put(HashMap* map, int key, int value) {
    int index = hash(key, map->capacity);
    while (map->entries[index].key != -1 && map->entries[index].key != key) {
        index = (index + 1) % map->capacity; // Linear probing
    }
    map->entries[index].key = key;
    map->entries[index].value = value;
}

// Function to get value from the HashMap
int get(HashMap* map, int key) {
    int index = hash(key, map->capacity);
    while (map->entries[index].key != -1) {
        if (map->entries[index].key == key) {
            return map->entries[index].value;
        }
        index = (index + 1) % map->capacity; // Linear probing
    }
    return -1; // Return -1 to indicate not found
}

// Function to find the minimum subarray length
int minSubarray(int* nums, int numsSize, int p) {

//*****************************************************************************
//** Add error handling for 0 and negative values in array here              **
//*****************************************************************************

    long long totalSum = 0;

    // Compute total sum of the array
    for (int i = 0; i < numsSize; i++) {
        totalSum += nums[i];
    }

    // Calculate the target remainder
    int k = totalSum % p;
    if (k == 0) {
        return 0; // Already divisible by p, no need to remove anything
    }

    HashMap* last = createHashMap(numsSize + 1); // Initialize HashMap
    put(last, 0, -1); // Initialize the case for the prefix sum

    long long prefixSum = 0;
    int minLength = INT_MAX;

    for (int i = 0; i < numsSize; i++) {
        prefixSum += nums[i];
        int currentRemainder = (prefixSum % p + p) % p;

        // Check for the case where removing a single element works
        if (nums[i] % p == k) {
            minLength = 1; // We can remove this single element
        }

        // Find the needed remainder
        int targetRemainder = (currentRemainder - k + p) % p;

        // Check if the needed remainder exists in the hash map
        int lastIndex = get(last, targetRemainder);
        if (lastIndex != -1) {
            minLength = (i - lastIndex < minLength) ? i - lastIndex : minLength;
        }

        // Store the current remainder with its index if not already present
        if (get(last, currentRemainder) == -1) {
            put(last, currentRemainder, i);
        }
    }

    free(last->entries);
    free(last); // Free the allocated memory

    return (minLength == INT_MAX) ? -1 : minLength;
}