export default function Home() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-orange-50 to-white flex flex-col">
      {/* Hero section */}
      <section className="flex flex-col items-center justify-center text-center flex-1 px-6 pt-10 pb-6">
        <div className="max-w-3xl">
          <div className="text-5xl font-extrabold text-gray-900 mb-4">
            “Push yourself, because no one else is going to do it for you.”
          </div>

          <p className="text-gray-500 mb-6">
            A complete platform to manage memberships, workouts, payments,
            trainers, and progress — all in one place.
          </p>

          <div className="flex justify-center gap-4 mb-12">
            <button className="bg-orange-500 text-white px-6 py-3 rounded-lg font-semibold hover:bg-orange-600 transition">
              Sign In
            </button>
            <button className="border border-gray-300 px-6 py-3 rounded-lg font-semibold text-gray-700 hover:border-orange-400 hover:text-orange-500 transition">
              Get Started
            </button>
          </div>
        </div>

        {/* Feature cards */}
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 max-w-6xl mx-auto">
          {[
            {
              icon: "👤",
              title: "User Management",
              desc: "Role-based access for members, trainers, and admins with secure authentication.",
            },
            {
              icon: "💳",
              title: "Membership Plans",
              desc: "Flexible subscription options with automatic renewal and expiry tracking.",
            },
            {
              icon: "📅",
              title: "Class Scheduling",
              desc: "Book classes, manage schedules, and track attendance seamlessly.",
            },
            {
              icon: "📈",
              title: "Progress Tracking",
              desc: "Track workouts, BMI, and performance improvement history.",
            },
            {
              icon: "💰",
              title: "Payment System",
              desc: "Secure payment processing with receipts and transaction history.",
            },
            {
              icon: "🏋️",
              title: "Workout Plans",
              desc: "Trainers can create personalized workout plans for members.",
            },
          ].map((card) => (
            <div
              key={card.title}
              className="bg-white border border-gray-200 rounded-xl shadow-sm p-6 text-left hover:shadow-md transition"
            >
              <div className="text-3xl text-orange-500 mb-3">{card.icon}</div>
              <h3 className="text-lg font-semibold text-gray-800 mb-1">
                {card.title}
              </h3>
              <p className="text-gray-500 text-sm leading-relaxed">
                {card.desc}
              </p>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}
