import { Link } from "react-router-dom";

export default function Home() {
  return (
    <div className="bg-black text-white min-h-screen font-[Poppins]">
      {/* Hero */}
      <section className="relative h-screen flex flex-col justify-center items-start px-10 md:px-24">
        <img
          src="https://media.istockphoto.com/id/1495388784/photo/sports-fitness-and-equipment-with-empty-room-of-gym-for-workout-health-and-background.jpg?s=612x612&w=0&k=20&c=xG-0wFkye7gcv8RXUsbQoBv1MaEZyvdYVFHShB0ghmU="
          alt="Gym Background"
          className="absolute inset-0 w-full h-full object-cover"
        />
        <div className="absolute inset-0 bg-gradient-to-r from-black/90 via-black/70 to-transparent" />
        <div className="relative z-10 max-w-3xl">
          <p className="text-sm tracking-[4px] text-yellow-400 uppercase mb-4">
            Elite Fitness Training
          </p>
          <h1 className="text-6xl md:text-7xl font-extrabold leading-tight">
            <span className="text-white">Shape it up!</span>
            <br />
            <span className="text-yellow-400">Get fit, don’t quit.</span>
          </h1>
          <p className="text-gray-300 mt-6 text-lg leading-relaxed">
            Today I will do what others won’t, so tomorrow I can accomplish what others can’t.
            Excellence then is not an act but a habit.
          </p>
          <Link
            to="/register"
            className="mt-10 inline-block bg-yellow-500 text-black px-10 py-3 rounded-full text-lg font-semibold hover:bg-yellow-400 transition duration-300"
          >
            Join Us Now
          </Link>
        </div>
      </section>

      {/* About */}
      <section className="py-24 bg-black text-center border-t border-neutral-800">
        <h2 className="text-4xl font-bold text-yellow-400 mb-6 uppercase tracking-wide">
          About SPORTIFY
        </h2>
        <p className="max-w-3xl mx-auto text-gray-400 text-lg leading-relaxed">
          SPORTIFY is your digital fitness hub. Build discipline, sharpen your mindset, and track
          your growth. Join a global community committed to strength, endurance, and excellence.
        </p>
      </section>
    </div>
  );
}
