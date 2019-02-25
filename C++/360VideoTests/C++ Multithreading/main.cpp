/*

Multithreaded application to test POSIX thread implementation in C++

Must be compiled with:  g++ -pthread -o <name> main.cpp

*/


#include <iostream>
#include <pthread.h>
#include <cstdlib>

using namespace std;

#define NUM_THREADS 3

void *PrintHello(void *threadID, int boop){
    long tid;
    tid = (long)threadID;
    cout << "Hello Worlds out there?" << endl;
    pthread_exit(NULL);
}

int main(){
    pthread_t threads[NUM_THREADS];
    int rc;

    for (int i = 0; i < NUM_THREADS; i++){
        cout << "creating thread " << i << endl;
        rc = pthread_create(&threads[i], NULL, PrintHello, (void *)i);

        if (rc){
            cout << "Error creating thread " << rc << endl;
            exit(-1);
        }
    }
    pthread_exit(NULL);
}