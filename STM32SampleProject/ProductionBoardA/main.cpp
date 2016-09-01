#include <cmath>

class PointBase
{
};

class Point : public PointBase
{
  public:
    float X;
    float Y;

    /**
     * Calculates the distance from 1 pointer to another.
     * @param p the other point to calculate distance to.
     */
    float DistanceTo (Point& p);
};

/**
 * Static Distance to
 * @param p2 second point  
 * @param p1 first point
 */
static void DistanceTo (Point& p1, Point& p2)
{
    
}

float Point::DistanceTo (Point& to)
{
    auto dx = fabsf (this->X - to.X);
    auto dy = fabsf (this->Y - to.Y);

    return sqrtf ((dx * dx) + (dy * dy));
}

float sum (float x, float y)
{
    return x + y;
}

int sum (int x, int y)
{
    return x + y;
}

int main (void)
{
    int x = 0;


    Point p;
    p.X = 1.25;
    p.Y = 2.45;

    Point p1;
    p1.X = 1.25;
    p1.Y = 2.45;

//    Dis
    p.DistanceTo ();
    while (true)
    {

        auto distance = p.DistanceTo (p1);

        x = sum (1, 2);
    }

    return x;
}